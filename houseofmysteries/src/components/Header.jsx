import React, { useState, useEffect, useCallback } from "react";
import { Link, useNavigate } from "react-router-dom";
import logo from "../assets/images/logo.png";
import axios from "axios";

const Header = () => {
    const navigate = useNavigate();
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [username, setUsername] = useState("");
    const [isAdmin, setIsAdmin] = useState(false);

    const checkAdminRole = useCallback(async (token, currentUsername) => {
        if (!token || !currentUsername) {
            setIsAdmin(false);
            return;
        }
        try {
            const response = await axios.get(`http://localhost:5131/Users/GetByUserName/${token},${currentUsername}`);
            setIsAdmin(response.data.roleId === 4);
        } catch (error) {
            console.error("Hiba a jogosultság lekérdezésekor a Headerben:", error);
            setIsAdmin(false);
        }
    }, []);

    // A handleStorageChange callback-ot a komponens fő testében definiáljuk,
    // de nem használhatjuk benne közvetlenül a useCallback-kal létrehozott checkAdminRole-t.
    // Ehelyett a checkAdminRole-t közvetlenül hívjuk meg az useEffect-ben.
    const handleStorageChange = useCallback((e) => {
        if (e.key === 'token' || e.key === 'username') {
            const newToken = localStorage.getItem('token');
            const newUsername = localStorage.getItem('username');
            if (newToken && newUsername) {
                setIsLoggedIn(true);
                setUsername(newUsername);
                checkAdminRole(newToken, newUsername);
            } else {
                setIsLoggedIn(false);
                setUsername("");
                setIsAdmin(false);
            }
        }
    }, [checkAdminRole]);

    useEffect(() => {
        const token = localStorage.getItem('token');
        const storedUsername = localStorage.getItem('username');
        if (token && storedUsername) {
            setIsLoggedIn(true);
            setUsername(storedUsername);
            checkAdminRole(token, storedUsername);
        } else {
            setIsLoggedIn(false);
            setUsername("");
            setIsAdmin(false);
        }

        window.addEventListener('storage', handleStorageChange);

        return () => {
            window.removeEventListener('storage', handleStorageChange);
        };
    }, [checkAdminRole, handleStorageChange]); // Hozzáadtuk a handleStorageChange-t a függőségekhez

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('username');
        localStorage.removeItem('loggedIn');
        setIsLoggedIn(false);
        setUsername("");
        setIsAdmin(false);
        navigate("/");
    };

    return (
        <header className="header">
            <nav className="nav-container">
                <Link to="/">
                    <img src={logo} alt="Rejtélyek Háza Logó" className="logo" />
                </Link>
                <ul className="menu">
                    <li><Link to="/" className="menu-button">Főoldal</Link></li>
                    <li><Link to="/about" className="menu-button">Rólunk</Link></li>
                    <li><Link to="/tracks" className="menu-button">Pályák és foglalás</Link></li>
                    <li><Link to="/prices" className="menu-button">Árak</Link></li>
                    <li><Link to="/reviews" className="menu-button">Vélemények</Link></li>
                    <li><Link to="/faq" className="menu-button">GYIK</Link></li>
                    <li><Link to="/contact" className="menu-button">Kapcsolat</Link></li>
                    <li><Link to="/gallery" className="menu-button">Galéria</Link></li>
                    <li><Link to="/privacy" className="menu-button">Adatvédelmi szabályzat</Link></li>
                    {isLoggedIn && <li><Link to="/profile" className="menu-button">Profil</Link></li>}
                    {isLoggedIn && isAdmin && <li><Link to="/admin" className="menu-button">Admin</Link></li>}

                </ul>
            </nav>

            <div className="auth-buttons">
                {isLoggedIn ? (
                    <>
                        <span>Üdv, </span><Link to="/profile" className="profile-link">{username}</Link><span>!</span>
                        <button onClick={handleLogout} className="logout-button">Kijelentkezés</button>
                    </>
                ) : (
                    <>
                        <Link to="/login" className="login-button">Belépés</Link>
                        <Link to="/register" className="register-button">Regisztráció</Link>
                    </>
                )}
            </div>
        </header>
    );
};

export default Header;