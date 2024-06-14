docker build -t kafka-tester .
docker run --rm --network=host kafka-tester