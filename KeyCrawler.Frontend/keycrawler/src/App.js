import React, { useState, useEffect } from "react";
import "./App.css";

import { HelloRequest } from "./Protos/greet_pb"
import { GreeterClient } from "./Protos/greet_grpc_web_pb"

var client = new GreeterClient("http://localhost:6060");

function App() {
  const [greet, setGreet] = useState("NO GREET :(")

  const getGreet = () => {
    console.log("greet!");

    var greetRequest = new HelloRequest();
    greetRequest.setName("Tomas");
    var stream = client.sayHellos(greetRequest, {});

    stream.on("data", (response) => setGreet(response.getMessage()));
  };

  useEffect(() => getGreet(), []);

  return (
    <div>
      Greets: {greet}
    </div>
  );
}

export default App;
