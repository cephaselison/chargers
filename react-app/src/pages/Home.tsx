import axios from "axios";
import React, { useEffect, useState } from "react";
import Installation from "../components/Installation";
import { ChargerDto, InstallationDto } from "../components/types";

function Home() {
  const [installations, setInstalations] = useState<InstallationDto[]>();

  const handleChargerConnect = (charger: ChargerDto) => {
    if (!installations) return;

    let installationCopy = [...installations];

    installationCopy.map((installation) => {
      if (installation.id === charger.installationId) {
        installation.chargers.map((c) => {
          if (c.id === charger.id) {
            charger.connected
              ? disconnectCharger(charger.id)
              : connectCharger(charger.id);
            charger.connected = !charger.connected;
          }
        });
      }
    });

    setInstalations(installationCopy);
    return;
  };

  // Todo: create service.ts with all api requests and axios
  const connectCharger = async (chargerId: string) => {
    axios
      .post(`http://localhost:5000/api/chargers/${chargerId}/connect`)
      .then((result: any) => {});
  };

  const disconnectCharger = async (chargerId: string) => {
    axios
      .post(`http://localhost:5000/api/chargers/${chargerId}/disconnect`)
      .then((result: any) => {});
  };

  useEffect(() => {
    axios.get("http://localhost:5000/api/installations").then((result: any) => {
      setInstalations(result.data);
    });
  }, []);

  return (
    <>
      {installations &&
        installations.map((installation) => (
          <Installation
            installation={installation}
            key={installation.id}
            handleChargerConnect={(charger) => handleChargerConnect(charger)}
          />
        ))}
    </>
  );
}

export default Home;
