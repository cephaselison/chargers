import { createSocket } from "dgram";
import { createContext, useContext, useEffect, useState } from "react";
import io, { Socket } from "socket.io-client";
import { InstallationDto } from "../components/types";
interface Context {
  socket: Socket;
  username?: string;
  setUsername: Function;
  messages?: { message: string; time: string; username: string }[];
  setMessages: Function;
  roomId?: string;
  rooms: object;
  installations: InstallationDto;
}

const EVENTS = {
  connection: "connection",
  CLIENT: {},
  SERVER: {
    INSTALLATION: "INSTALLATION",
  },
};

const socket = io("http://localhost:5000", {
  path: "api/installations/",
});

const SocketContext = createContext<Context>({
  socket,
  setUsername: () => false,
  setMessages: () => false,
  rooms: {},
  messages: [],
  installations: {
    id: "",
    chargers: [],
    name: "",
    circuitBreakerCurrent: 0,
    circuitBreakerTripped: false,
  },
});

function SocketsProvider(props: any) {
  const [username, setUsername] = useState("");
  const [roomId, setRoomId] = useState("");
  const [rooms, setRooms] = useState({});
  const [messages, setMessages] = useState([]);
  const [installations, setInstallations] = useState<InstallationDto>();

  useEffect(() => {
    window.onfocus = function () {};
  }, []);

  socket.on(EVENTS.SERVER.INSTALLATION, (value) => {});

  return (
    <SocketContext.Provider
      value={{
        socket,
        username,
        setUsername,
        rooms,
        roomId,
        messages,
        setMessages,
      }}
      {...props}
    />
  );
}

export const useSockets = () => useContext(SocketContext);

export default SocketsProvider;
