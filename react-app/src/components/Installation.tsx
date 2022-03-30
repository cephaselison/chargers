import axios from "axios";
import React, { useEffect, useState } from "react";
import { useSockets } from "../context/socket.context";
import Charger from "./Charger";
import { ChargerDto, InstallationDto } from "./types";

interface IPropComponent {
  installation: InstallationDto;
  handleChargerConnect: (charger: ChargerDto) => void;
}

const Installation = (props: IPropComponent) => {
  const { installation, handleChargerConnect } = props;

  return (
    <div className="container">
      <div className="row">
        <h1>{installation.name}</h1>
      </div>

      {installation.circuitBreakerTripped && (
        <div className="alert">Circuit breaker has been tripped!</div>
      )}

      <div className="chargers">
        <ul>
          {installation.chargers &&
            installation.chargers.map((charger) => (
              <li className="charger" key={charger.id}>
                {charger.name} (charged: {charger.allocatedCurrent}kWh){" "}
                <ul>
                  <Charger
                    charger={charger}
                    handleChargerConnect={(charger) =>
                      handleChargerConnect(charger)
                    }
                  />
                </ul>
              </li>
            ))}
        </ul>
      </div>
    </div>
  );
};

export default Installation;
