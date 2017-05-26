# 프로젝트 설명
 .NET Core C#을 사용하여 티스토리 백업 파일을 원하는 형태로 컨버팅시키는 프로젝트입니다. 기본적으로 ## 프로젝트에서 사용할 형태의 metadata/markdown 파일로 만들어내는 것을 목표로 합니다

# 프로젝트 내용
 * 글 하나당 하나의 디렉토리를 생성하고, index.md, metadata.xml 파일 생성
 * 첨부 파일들을 해당 글의 디렉토리에 저장
 * 글 본문을 index.md - markdown 파일로 저장
 * metadata.xml 파일에 제목/생성 시각등을 저장

# 사용법
 * pandoc( http://pandoc.org/ )이 설치되어 있고, path에 설정되어 있어야 함
```
dotnet run tistory_backup.xml output_directory_name
```
