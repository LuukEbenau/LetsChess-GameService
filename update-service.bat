@ECHO OFF
ECHO Lets update the service!
docker build . -t sacation/letschess-gameservice
docker push sacation/letschess-gameservice