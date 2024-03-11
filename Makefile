install-packages:
	dotnet add package System.Security.Cryptography.Pkcs --version 8.0.0

run:
	dotnet run input.txt sample_cert.pfx 1234 output.txt

build-win:
	dotnet publish -r win-x64 

build-linux:
	dotnet publish -r linux-x64 