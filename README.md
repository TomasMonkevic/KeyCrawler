# How to run #

In terminal run:

`docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d --build`

# Useful information #

To test gRPC calls use grpcurl. Example:

`grpcurl -proto KeyCrawler.GrpcApi\Protos\greet.proto -plaintext -d '{\"name\": \"World\"}' localhost:6000 greet.Greeter/SayHello`