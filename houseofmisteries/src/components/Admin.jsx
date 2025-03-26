import React, { useState, useEffect, useCallback } from "react";
import Header from "./Header";
import Footer from "./Footer";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { FaEdit, FaTrash } from "react-icons/fa";




const Admin = () => {
    const navigate = useNavigate();
    const [rooms] = useState([]);
    const [selectedRoomId, setSelectedRoomId] = useState("");
    const [selectedDate, setSelectedDate] = useState("");
    const [selectedBookingDate1, setSelectedBookingDate1] = useState("");
    const [bookingsList, setBookingsList] = useState([]);
    const [users, setUsers] = useState([]);
    const [selectedRoomIdForMaintenance, setSelectedRoomIdForMaintenance] = useState("");       
    const [error, setError] = useState("");
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [selectedRoomIdForCompetition, setSelectedRoomIdForCompetition] = useState("");
    const [bookingDate, setBookingDate] = useState("");
    const [isAvailable, setIsAvailable] = useState(false);
    const [competitionMessage, setCompetitionMessage] = useState("");
    const [hours, setHours] = useState('00');
    const [minutes, setMinutes] = useState('00');
    const [seconds, setSeconds] = useState('00');
    const [successMessage, setSuccessMessage] = useState("");
    const [bookingsError, setBookingsError] = useState("");
    const [maintenanceError, setMaintenanceError] = useState("");
    const [competitionError, setCompetitionError] = useState("");
    
    
    

    const handleCompetitionRoomChange = (event) => {
        setSelectedRoomIdForCompetition(event.target.value);
        setCompetitionMessage("");
    };

    const handleHoursChange = (event) => {
        setHours(event.target.value);
    };

    const handleMinutesChange = (event) => {
        setMinutes(event.target.value);
    };

    const handleSecondsChange = (event) => {
        setSeconds(event.target.value);
    };


    const loadInitialData = useCallback(async () => {
        const token = localStorage.getItem("token");
        await Promise.all([getUsers(token)]);
    }, []);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) {
            navigate("/login");
            setIsLoggedIn(false);
            return;
        }
        setIsLoggedIn(true);
        loadInitialData();
    }, [navigate, loadInitialData]);

    
      const getUsers = async () => {
        const token = localStorage.getItem("token");
        try {
          const response = await axios.get(`http://localhost:5000/Users/${token}`);
          setUsers(response.data);
        } catch {
          setError("Nem sikerült betölteni a felhasználókat.");
        }
      };

      const loadBookings = async () => {
        const token = localStorage.getItem("token");
        if (!selectedRoomId || !selectedDate) {
            setError("Válassz ki egy szobát és egy dátumot!");
            return;
        }

        const url = `http://localhost:5000/Booking/CheckBooking${token}`;

        try {
            const response = await axios.get(url, {
                params: {
                    day: selectedDate,
                    roomId: selectedRoomId
                }
            });
            setBookingsList(response.data);
        } catch (error) {
            setBookingsError("Nem sikerült lekérdezni a foglalásokat.");
            console.error("Foglalás lekérdezési hiba:", error);
        }
    };

    const deleteBooking = async (bookingId) => {
        const token = localStorage.getItem("token");
        if (!token) {
            throw new Error("Nincs bejelentkezve!");
        }
    
        try {
            await axios.delete(`http://localhost:5000/Booking/${token}`, {
                params: {
                    bookingId: bookingId
                }
            });
            return { success: true };
        } catch (error) {
            console.error("Foglalás törlési hiba:", error);
            throw new Error("Nem sikerült törölni a foglalást.");
        }
    };

    const clearBooking = async (bookingDate) => {
        const token = localStorage.getItem("token");
        if (!token) {
            throw new Error("Nincs bejelentkezve!");
        }
    
        try {
            await axios.put(`http://localhost:5000/Booking/ClearBooking/${token}`, {
                bookingDate: bookingDate,
                
            });
            return { success: true };
        } catch (error) {
            console.error("Foglalás felszabadítási hiba:", error);
            throw new Error("Nem sikerült felszabadítani a foglalást.");
        }
    };



        const toggleMaintenance = async () => {
            const token = localStorage.getItem("token");
            if (!selectedRoomIdForMaintenance || !selectedBookingDate1) {
                setError("Válassz egy szobát, egy dátumot és egy időpontot!");
                return;
            }
        
            try {
                ;
        
         const maintenanceData = {
                    bookingDate: selectedBookingDate1,
                    roomId: selectedRoomIdForMaintenance,
                    teamId: "1", // vagy bármilyen más teamId
                    comment: "Karbantartás",
                };
        
                console.log("selectedmaintenanceData:", maintenanceData);
        
                const response = await axios.post(`http://localhost:5000/Booking/NewBooking/${token}`, maintenanceData);
        
                if (response.status === 200) {
                    setMaintenanceError("");
                    setSuccessMessage(response.data); // Backend üzenetének megjelenítése
                } else {
                    setMaintenanceError("Hiba a karbantartás beállításakor.");
                    setSuccessMessage(""); // Sikeres üzenet törlése, ha hiba van
                }
            } catch (error) {
                setMaintenanceError(`Nem sikerült beküldeni: ${error.message}`);
                setSuccessMessage(""); // Sikeres üzenet törlése, ha hiba van
            }
        };
         
          const handleSaveCompetition = async () => {
            const token = localStorage.getItem("token");
            const resultTime = `${hours}:${minutes}:${seconds}`;
    
            try {
                const competitionData = {
                    bookingDate: bookingDate,
                    roomId: selectedRoomIdForCompetition,
                    result: resultTime,
                };
        
                const response = await axios.put(
                    `http://localhost:5000/Booking/SaveResult/${token}`,
                    competitionData 
                );
        
                if (response.status === 200) {
                    setCompetitionMessage("Versenyadatok sikeresen mentve!");
                    setCompetitionError(""); // Hibaüzenet törlése, ha sikeres
                } else {
                    setCompetitionMessage("Hiba a versenyadatok mentésekor.");
                    setCompetitionError("Hiba a versenyadatok mentésekor.");
                }
            } catch (error) {
                setCompetitionMessage("Hiba a versenyadatok mentésekor: " + error.message);
                setCompetitionError("Hiba a versenyadatok mentésekor: " + error.message);
                console.error(error);
            }
        };


    return isLoggedIn ? (
        <>
            <Header />
            <main className="admin-container">
                <h1>Adminisztrációs felület</h1>
                {error && <p className="error-message">{error}</p>}

                <div className="admin-grid">
                    {/* 1. Foglalások kezelése */}
                    <section className="admin-section">
                <h2>Foglalások kezelése</h2>
                <label>Pálya:</label>
                <select
                    value={selectedRoomId}
                    onChange={(e) => setSelectedRoomId(e.target.value)}
                >
                    <option value="">Válassz egy szobát</option>
                    <option value="1">Menekülés az iskolából</option>
                    <option value="2">A pedellus bosszúja</option>
                    <option value="3">A tanári titkai</option>
                    <option value="4">A takarítónő visszanéz</option>
                    <option value="5">Szabadulás Kódja</option>
                    <option value="6">Időcsapda</option>
                    <option value="7">KódX Szoba</option>
                    <option value="8">Kalandok Kamrája</option>
                    <option value="9">Titkok Labirintusa</option>
                </select>
                <label>Dátum:</label>
                <input
                    type="date"
                    value={selectedDate}
                    onChange={(e) => setSelectedDate(e.target.value)}
                />
                <button onClick={loadBookings}>Foglalások lekérése</button>
                {bookingsError && <p style={{ color: 'red' }}>{bookingsError}</p>}
                {bookingsList.length > 0 && (
                    <table className="bookings-table">
                        <thead>
                            <tr>
                                <th>Foglalás időpontja</th>
                                <th>Csapat ID</th>                                
                                <th>Megjegyzés</th>
                                <th>Törlés</th>
                            </tr>
                        </thead>
                        <tbody>
                            {bookingsList.map((booking) => (
                                <tr key={booking.bookingId}>
                                    <td>{new Date(booking.bookingDate).toLocaleString()}</td>
                                    <td>{booking.teamId}</td>                                    
                                    <td>{booking.comment}</td>
                                    <td>
                                        <button onClick={() => deleteBooking(booking.bookingId)}>Törlés</button>
                                        <button onClick={() => clearBooking(booking.bookingId)}>Felszabadít</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </section>

                    <section className="admin-section">
                        <h2>Szobák karbantartása</h2>
                        <label>Válassz egy szobát:</label>
                        <select onChange={(e) => setSelectedRoomIdForMaintenance(e.target.value)}>
                        <option value="">Válassz egy szobát</option>
                            <option value="1">Menekülés az iskolából</option>
                            <option value="2">A pedellus bosszúja</option>
                            <option value="3">A tanári titkai</option>
                            <option value="4">A takarítónő visszanéz</option>
                            <option value="5">Szabadulás Kódja</option>
                            <option value="6">Időcsapda</option>
                            <option value="7">KódX Szoba</option>
                            <option value="8">Kalandok Kamrája</option>
                            <option value="9">Titkok Labirintusa</option>  
                        </select>
                        <label>Dátum:</label>
                        <input type="datetime-local" value={selectedBookingDate1} onChange={(e) => setSelectedBookingDate1(e.target.value)} />
                        <button onClick={toggleMaintenance}>
                            Karbantartás beállítása
                        </button>
                        {maintenanceError && <p style={{ color: 'red' }}>{"Hiba a karbantartás beállítása közben!"}</p>}
                        {successMessage && <p style={{ color: 'green' }}>{"Karbantartás sikeresen beállítva!"}</p>}
                    </section>

                    {/* 3. Felhasználók kezelése */}
                    <section className="admin-section">
                        <h2>Felhasználók kezelése</h2>
                        
                        <table className="user-table">
                            <thead>
                            <tr>
                                <th>Név</th>
                                <th>Email</th>
                                <th>Telefon</th>
                                <th>Jogosultság</th>
                                <th>Csapat ID</th>
                                <th>Műveletek</th>
                            </tr>
                            </thead>
                            <tbody>
                            {users.map((user) => (
                                <tr key={user.UserId}>
                                <td>{user.realName}</td>
                                <td>{user.email}</td>
                                <td>{user.phone}</td>
                                <td>{user.roleId}</td>
                                <td>{user.teamId}</td>
                                
                                <td>
                                    <div>
                                    <div className="icon-button edit" onClick={onclick}>
                                        <FaEdit />
                                    </div>
                                    <div className="icon-button delete" onClick={onclick}>
                                        <FaTrash />
                                    </div>
                                    </div>
                                </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                        <button onClick={getUsers}>Felhasználók betöltése</button>
                        </section>

                    {/* 4. Versenytábla kezelése */}
                   <section className="admin-section">
                <h2>Versenytábla kezelése</h2>
                <label>Szoba:</label>
                <select
                    value={selectedRoomIdForCompetition}
                    onChange={handleCompetitionRoomChange}
                >
                            
                            <option value="">Válassz egy szobát</option>
                                <option value="1">Menekülés az iskolából</option>
                                <option value="2">A pedellus bosszúja</option>
                                <option value="3">A tanári titkai</option>
                                <option value="4">A takarítónő visszanéz</option>
                                <option value="5">Szabadulás Kódja</option>
                                <option value="6">Időcsapda</option>
                                <option value="7">KódX Szoba</option>
                                <option value="8">Kalandok Kamrája</option>
                                <option value="9">Titkok Labirintusa</option>
                            {rooms.map((room) => (
                                <option key={room.RoomId} value={room.RoomId}>
                                    {room.Name}
                                </option>
                            ))}
                       </select>
                            <label>Foglalt időpont:</label>
                            <input type="datetime-local" value={bookingDate} onChange={(e) => setBookingDate(e.target.value)} />

                            <label>Eredmény:</label>
                            <input
                                type="number"
                                value={hours}
                                onChange={handleHoursChange}
                                min="0"
                                max="23"
                            />
                            :
                            <input
                                type="number"
                                value={minutes}
                                onChange={handleMinutesChange}
                                min="0"
                                max="59"
                            />
                            :
                            <input
                                type="number"
                                value={seconds}
                                onChange={handleSecondsChange}
                                min="0"
                                max="59"
                            />

                            <label>Kijutottak?</label>
                            <input type="checkbox" checked={isAvailable} onChange={(e) => setIsAvailable(e.target.checked)} />

                            <button onClick={handleSaveCompetition}>Hozzáadás</button>

                            {competitionError && <p style={{ color: 'red' }}>{competitionError}</p>}
                        {competitionMessage && <p>{competitionMessage}</p>}
                            </section>
                </div>
            </main>
            <Footer />
        </>
    ) : null;
};

export default Admin;