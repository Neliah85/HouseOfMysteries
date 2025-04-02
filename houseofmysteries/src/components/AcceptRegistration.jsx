import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import axios from "axios";
import Header from "./Header";
import Footer from "./Footer";

const AcceptRegistration = () => {
    const [message, setMessage] = useState("Regisztráció megerősítése folyamatban...");
    const [felhasznaloNev, setFelhasznaloNev] = useState(null);
    const [email, setEmail] = useState(null);
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const user = searchParams.get("felhasznaloNev");
        const userEmail = searchParams.get("email");

        console.log("Kinyert felhasználónév:", user);
        console.log("Kinyert email:", userEmail);

        if (user && userEmail) {
            setFelhasznaloNev(user);
            setEmail(userEmail);
            setMessage(`Kérlek, erősítsd meg a regisztrációdat '${userEmail}' email címmel!`);
        } else {
            setMessage("Érvénytelen regisztrációs link.");
        }
    }, [location.search]);

    const handleConfirmRegistration = async () => {
        if (felhasznaloNev && email) {
            console.log("POST kérés indítása előtt...");
            try {
                const response = await axios.post("http://localhost:5131/Registry/Confirm", {
                    loginName: felhasznaloNev,
                    email: email
                });
                console.log("POST kérés sikeres:", response.data);
                setMessage("Sikeresen megerősítetted a regisztrációdat!");
                setTimeout(() => {
                    navigate("/profile");
                }, 3000);
            } catch (error) {
                console.error("Hiba a regisztráció megerősítése során:", error);
                setMessage("Hiba történt a regisztráció megerősítése során. Kérlek, próbáld újra később.");
            }
        } else {
            setMessage("Hiba: Érvénytelen felhasználónév vagy email.");
        }
    };

    return (
        <div>
            <Header />
            <main>
                <h1>{message}</h1>
                {felhasznaloNev && email && (
                    <button type="button" className="accept-button" onClick={handleConfirmRegistration}>
                        Regisztráció megerősítése
                    </button>
                )}
            </main>
            <br /><br /><br /><br /><br />
            <Footer />
        </div>
    );
};

export default AcceptRegistration;
