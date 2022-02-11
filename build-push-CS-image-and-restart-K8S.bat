docker build -t 146353676/commandservice -f .\CommandServiceDockerfile  .
docker push 146353676/commandservice
kubectl rollout restart deployment command-deployment