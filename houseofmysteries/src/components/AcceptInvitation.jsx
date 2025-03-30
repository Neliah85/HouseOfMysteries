import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import axios from "axios";
import Header from "./Header";
import Footer from "./Footer";

const AcceptInvitation = () => {
    const [message, setMessage] = useState("Meghívás elfogadása...");
    const [felhasznaloNev, setFelhasznaloNev] = useState(null);
    const [teamName, setTeamName] = useState(null);
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const user = searchParams.get("felhasznaloNev");
        const team = searchParams.get("teamName");

        console.log("Kinyert felhasználónév:", user);
        console.log("Kinyert csapatnév:", team);

        if (user && team) {
            setFelhasznaloNev(user);
            setTeamName(team);
            setMessage(`Kérlek, erősítsd meg a meghívást a(z) '${team}' csapatba!`);
        } else {
            setMessage("Érvénytelen meghívó link.");
        }
    }, [location.search]);

    const handleAcceptInvitation = async () => {
        if (felhasznaloNev && teamName) {
            console.log("PUT kérés indítása előtt..."); // Új console log
            try {
                const response = await axios.put(
                    `http://localhost:5131/Teams/AcceptInvitation/${felhasznaloNev},${teamName}`
                );
                console.log("PUT kérés sikeres:", response.data);
                setMessage("Sikeresen elfogadtad a meghívást!");
                setTimeout(() => {
                    navigate("/profile");
                }, 3000);
            } catch (error) {
                console.error("Hiba a meghívás elfogadásakor:", error);
                setMessage("Hiba történt a meghívás elfogadása során. Kérlek, próbáld újra később.");
            }
        } else {
            setMessage("Hiba: Érvénytelen felhasználónév vagy csapatnév.");
        }
    };

    return (
        <div>
            <Header />
            <main>
                <h1>{message}</h1>
                {felhasznaloNev && teamName && (
                    <button type="button" className="accept-button" onClick={handleAcceptInvitation}>Meghívás megerősítése</button>
                )}
            </main>
            <br></br>
            <br></br>
            <br></br>
            <br></br>
            <br></br>
            <Footer />
        </div>
    );
};

export default AcceptInvitation;