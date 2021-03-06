﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Threading.Tasks;

namespace TistoryConvertor
{
    class Program
    {
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                PrintUsage();
                Environment.Exit(1);
            }
            string data_filename = args[0];
            string output_directory_name = args[1];
            Console.WriteLine("data_filename: {0}", data_filename);
            Console.WriteLine("output_directory_name: {0}", output_directory_name);

            var post_output = new PostOutput.Wimyblog(output_directory_name);
            Convert(data_filename, post_output);
        }

        public static void Convert(string data_filename, IPostOutput post_output)
        {
            if (File.Exists(data_filename) == false)
            {
                Console.WriteLine("Cannot find data file: {0}", data_filename);
                Environment.Exit(1);
            }
            using (var stream = File.OpenRead(data_filename))
            {
                var xml_document = new XmlDocument();
                xml_document.Load(stream);
                XmlNodeList elements = xml_document.GetElementsByTagName("post");
                List<XmlNode> xml_nodes = new List<XmlNode>();
                foreach (XmlNode node in elements)
                {
                    xml_nodes.Add(node);
                }
                Parallel.ForEach(xml_nodes, (XmlNode post_element) =>
                {
                    var post = ParseUtil.ParsePost(post_element);
                    post_output.OnPost(post);
                });
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: dotnet run data_filename output_directory_name");
        }
    }
}
