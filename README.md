# How to run #

In terminal run:

`docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d --build`

# Useful information #

To test gRPC calls use grpcurl. Example:

`grpcurl -proto KeyCrawler.GrpcApi\Protos\greet.proto -plaintext -d '{\"name\": \"World\"}' localhost:6000 greet.Greeter/SayHello`

To generate client code from protobuf use protoc for JS. Example:

`protoc -I.\..\..\KeyCrawler.GrpcApi\Protos\. greet.proto --js_out=import_style=commonjs:./src/Protos/. --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./src/Protos/.`

First time, this is bit of a pain to setup. For more information go to https://github.com/grpc/grpc-web