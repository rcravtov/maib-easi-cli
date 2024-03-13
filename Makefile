install-packages:
	dotnet add package System.Security.Cryptography.Pkcs --version 8.0.0

run-hash:
	dotnet run hash input.txt sample_cert.pfx 1234 output.txt

run-base64:
	dotnet run base64 input.txt output.txt

build-win:
	dotnet publish -r win-x64 

build-linux:
	dotnet publish -r linux-x64 