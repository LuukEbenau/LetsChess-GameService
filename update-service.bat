@ECHO OFF
ECHO Lets update the service!
cd ../
docker build -t sacation/letschess-gameservice  . -f LetsChess-GameService/Dockerfile
docker push sacation/letschess-gameservice