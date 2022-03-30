import React from "react";
import { ChargerDto as ChargerDto } from "./types";

interface IPropComponent {
  charger: ChargerDto;
  handleChargerConnect: (charger: ChargerDto) => void;
}

function Charger(props: IPropComponent) {
  const { charger, handleChargerConnect } = props;

  return (
    <>
      <li>
        <button
          onClick={() => handleChargerConnect(charger)}
          type="button"
          className="charge-connect"
        >
          <span>{!charger.connected ? "Connect" : "Disconnect"}</span>
        </button>
      </li>
    </>
  );
}

export default Charger;
