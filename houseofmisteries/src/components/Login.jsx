import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";
import CryptoJS from 'crypto-js';

const Login = () => {
    const navigate = useNavigate();
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            // Salt lekérése
            const saltResponse = await axios.post(`http://localhost:5000/Login/GetSalt/${userName}`);
            const salt = saltResponse.data;

            // Hash generálás
            let passwordWithSalt = password + salt;
            let hash = CryptoJS.SHA256(passwordWithSalt).toString(CryptoJS.enc.Hex);

            // Bejelentkezés
            const loginResponse = await axios.post("http://localhost:5000/Login", {
                LoginName: userName,
                TmpHash: hash,
            });

            if (loginResponse.status === 200) {
                const token = loginResponse.data.token; 

                // Token tárolása localStorage-ban
                localStorage.setItem("token", token);
                localStorage.setItem("username", userName);
                localStorage.setItem("loggedIn", true);
                console.log("Token: ", token);
                console.log("Username: ", userName);


                setError("");
                setSuccessMessage("Sikeres bejelentkezés!");

                setTimeout(() => {
                    setSuccessMessage("");
                    navigate("/");
                }, 2000);
            } else {
                setError("Hiba történt a bejelentkezéskor!");
            }
        } catch (error) {
            console.error("Hiba a bejelentkezés során:", error);
            setError("Hiba történt a bejelentkezés során.");
        }
    };

    return (
        <>
            <Header />
            <main className="auth-container">
                <h2>Belépés</h2>
                {error && <p className="error">{error}</p>}
                {successMessage && <p className="success">{successMessage}</p>}

                <form onSubmit={handleLogin}>
                    <label>Felhasználónév:</label>
                    <input type="text" value={userName} onChange={(e) => setUserName(e.target.value)} required />

                    <label>Jelszó:</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />

                    <button type="submit">Belépés</button>
                </form>
            </main>
            <Footer />
        </>
    );
};

export default Login;