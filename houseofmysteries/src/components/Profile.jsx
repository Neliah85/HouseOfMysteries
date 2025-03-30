import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";

const Profile = () => {
    const [userData, setUserData] = useState({
        realName: "",
        nickName: "",
        email: "",
        phone: "",
        teamId: null,
        teamName: "",
        userId: 0,
        roleId: 0,
    });
    const [password, setPassword] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const [addUserNickname, setAddUserNickname] = useState("");
    const [addUserError, setAddUserError] = useState("");
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const userName = localStorage.getItem("userName");

    useEffect(() => {
        if (!token) {
            navigate("/login");
            return;
        }

        const getUserData = async () => {
            try {
                const userName = localStorage.getItem("username");
                console.log("Token a felhasználói adatok lekérdezéséhez:", token);
                console.log("Felhasználónév a felhasználói adatok lekérdezéséhez:", userName);
                const response = await axios.get(`http://localhost:5131/Users/GetByUserName/${token},${userName}`);
                console.log("Felhasználói adatok sikeresen lekérdezve:", response.data);
                setUserData({
                    realName: response.data.realName,
                    email: response.data.email,
                    phone: response.data.phone,
                    teamId: response.data.teamId,
                    teamName: response.data.teamName || "",
                    nickName: response.data.nickName,
                    userId: response.data.userId,
                    hash: response.data.hash,
                    salt: response.data.salt,
                    roleId: response.data.roleId !== null ? response.data.roleId : 0,
                });
            } catch (error) {
                console.error("Hiba a felhasználói adatok lekérésekor:", error);
                navigate("/login");
            }
        };
        getUserData();
    }, [navigate, token, userName]);

    const handleSave = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem("token");
            const updatedData = {
                ...userData,
                password: password || undefined,
                userId: userData.userId,
                roleId: userData.roleId,
            };
            console.log("Token a profil frissítéséhez:", token);
            console.log("Frissítendő adatok:", updatedData);
            await axios.put(`http://localhost:5131/Users/UpdateUser/${token}`, updatedData);
            console.log("Profil sikeresen frissítve!");
            setSuccessMessage("Profil sikeresen frissítve!");
            setTimeout(() => {
                setSuccessMessage("");
            }, 3000);
        } catch (error) {
            console.error("Hiba a profil frissítésekor:", error);
            setSuccessMessage("");
        }
    };

    const handleSendInvitationToTeam = async () => {
        if (!userData.teamName) { // Fontos: a csapatnév kell itt, nem az ID
            setAddUserError("Először regisztrálj egy csapatot az időpont foglalásnál!");
            return;
        }

        if (!addUserNickname) {
            setAddUserError("Kérlek, add meg a meghívandó felhasználó felhasználónevét!");
            return;
        }

        try {
            const token = localStorage.getItem("token");
            const teamName = userData.teamName;
            const requestUserName = localStorage.getItem("username");

            console.log("Token a meghívó küldéséhez:", token);
            console.log("Kérést indító felhasználó:", requestUserName);
            console.log("Meghívandó felhasználó:", addUserNickname);
            console.log("Csapatnév:", teamName);

            const response = await axios.put(
                `http://localhost:5131/Teams/AddUserToTeam/${token},${addUserNickname},teamName`,
                {},
                {
                    params: {
                        teamName: teamName, 
                    },
                }
            );
            console.log("Meghívó sikeresen elküldve:", response.data);
            setSuccessMessage(`Meghívó sikeresen elküldve a felhasználónak: '${addUserNickname}'!`);
            setAddUserError("");
            setAddUserNickname("");
            setTimeout(() => {
                setSuccessMessage("");
            }, 6000);
        } catch (error) {
            console.error("Hiba a meghívó küldésekor:", error);
            setAddUserError("Nem sikerült elküldeni a meghívót a felhasználónak.");
            if (error.response) {
                console.error("Backend válasz:", error.response.data);
                console.error("Backend státuszkód:", error.response.status);
            }
        }
    };

    return (
        <>
            <Header />
            <main className="profile-container">
                <h2>Profil</h2>
                <form onSubmit={handleSave}>
                    <label>Valódi név:</label>
                    <input type="text" value={userData.realName} onChange={(e) => setUserData({ ...userData, realName: e.target.value })} />

                    <label>Felhasználónév:</label>
                    <input type="text" value={userData.nickName} disabled />

                    <label>Email:</label>
                    <input type="email" value={userData.email} onChange={(e) => setUserData({ ...userData, email: e.target.value })} />

                    <label>Telefon:</label>
                    <input type="text" value={userData.phone} onChange={(e) => setUserData({ ...userData, phone: e.target.value })} />

                    <label>Új jelszó (opcionális):</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />

                    {successMessage && <p className="success-message">{successMessage}</p>}

                    <button type="submit">Mentés</button>
                </form>

                <div className="team-info">
                    <h3>Csapat információ</h3>
                    {userData.teamName ? (
                        <p>Jelenlegi csapat: {userData.teamName}</p>
                    ) : (
                        <p>Nem vagy tagja csapatnak.</p>
                    )}
                </div>

                <div className="add-user-to-team">
                    <h3>Meghívó küldése felhasználónak a csapathoz</h3>
                    {userData.teamId ? (
                        <>
                            <label>Felhasználónév:</label>
                            <input
                                type="text"
                                value={addUserNickname}
                                onChange={(e) => setAddUserNickname(e.target.value)}
                                placeholder="Meghívandó felhasználó felhasználóneve"
                            />
                            <br></br>
                            <br></br>
                            
                            <button type="submit" onClick={handleSendInvitationToTeam}>
                                Meghívó küldése
                            </button>
                            {addUserError && <p className="error-message">{addUserError}</p>}
                        </>
                    ) : (
                        <p className="warning-message">
                            Először regisztrálj egy csapatot az időpont foglalásnál, hogy felhasználókat hívhass meg!
                        </p>
                    )}
                </div>
            </main>
            <Footer />
        </>
    );
};

export default Profile;