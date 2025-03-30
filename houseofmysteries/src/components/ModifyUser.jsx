import React, { useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import axios from "axios";

const ModifyUser = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { user } = location.state || {};

    const [formData, setFormData] = useState({
        realName: user?.realName || "",
        nickName: user?.nickName || "",
        email: user?.email || "",
        phone: user?.phone || "",
        roleId: user?.roleId || "",
        teamId: user?.teamId || "",
        userId: user?.userId || "",   // Hozzáadva az azonosító kezelése
        hash: "",
        salt: ""
    });

    const [error, setError] = useState("");

    useEffect(() => {
        const fetchUserData = async () => {
            const token = localStorage.getItem("token");
            if (!user?.nickName) {
                setError("Felhasználónév nem található.");
                return;
            }

            try {
                const response = await axios.get(`http://localhost:5131/Users/GetByUserName/${token},${user.nickName}`);
                setFormData({
                    ...response.data,
                    userId: response.data.userId,   // Az azonosító biztosan bekerül az állapotba
                    hash: response.data.hash,
                    salt: response.data.salt
                });
            } catch (error) {
                console.error("Hiba a felhasználói adatok lekérésekor:", error);
                setError("Nem sikerült lekérni a felhasználói adatokat.");
            }
        };

        if (user?.nickName) {
            fetchUserData();
        }
    }, [user]);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = localStorage.getItem("token");

        if (!formData.userId) {
            setError("Hibás felhasználói azonosító.");
            return;
        }

        try {
            await axios.put(`http://localhost:5131/Users/${token},${formData.userId}`, formData);
            navigate(-1); // Visszanavigálás az előző oldalra
        } catch (error) {
            console.error("Hiba a módosítás során:", error);
            setError("Hiba történt a módosítás közben.");
        }
    };

    return (
        <div className="profile-container">
            <h2>Felhasználó módosítása</h2>
            <form onSubmit={handleSubmit}>
                <label>
                    Név:
                    <input type="text" name="realName" value={formData.realName} onChange={handleChange} required />
                </label>
                <label>
                    Felhasználónév:
                    <input type="text" name="nickName" value={formData.nickName} onChange={handleChange} required />
                </label>
                <label>
                    Email:
                    <input type="email" name="email" value={formData.email} onChange={handleChange} required />
                </label>
                <label>
                    Telefon:
                    <input type="text" name="phone" value={formData.phone} onChange={handleChange} required />
                </label>
                <label>
                    Jogosultság:
                    <input type="text" name="roleId" value={formData.roleId} onChange={handleChange} required />
                </label>
                <label>
                    Csapat ID:
                    <input type="text" name="teamId" value={formData.teamId} onChange={handleChange} required />
                </label>
                {error && <p className="error-message">{error}</p>}
                <button type="submit">Mentés</button>
                <button type="button" onClick={() => navigate(-1)}>Mégse</button>
            </form>
        </div>
    );
};

export default ModifyUser;