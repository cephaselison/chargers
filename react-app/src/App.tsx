import React from "react";
import logo from "./logo.svg";
import "./App.css";
import SocketsProvider from "./context/socket.context";
import Home from "./pages/Home";

function App() {
  return (
    <SocketsProvider>
      <Home />
    </SocketsProvider>
  );
}

export default App;
