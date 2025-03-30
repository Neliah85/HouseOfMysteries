import React, { useState } from "react";
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
        teamId: user?.teamId || ""
    });

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = localStorage.getItem("token");
        try {
            await axios.put(`http://localhost:5131/Users/${token},${user.UserId}`, formData);
            alert("Felhasználó sikeresen módosítva");
            navigate(-1); // Visszanavigálás az előző oldalra
        } catch (error) {
            console.error("Hiba a módosítás során:", error);
            alert("Hiba történt a módosítás közben");
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
                    <input type="text" name="nickname" value={formData.nickName} onChange={handleChange} required />
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
                <button type="submit">Mentés</button>
                <button type="button" onClick={() => navigate(-1)}>Mégse</button>
            </form>
        </div>
    );
};

export default ModifyUser;
