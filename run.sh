docker image rm kafka-tester-consumer
docker build --no-cache -t kafka-tester-consumer .
docker run --rm --network=host kafka-tester-consumer