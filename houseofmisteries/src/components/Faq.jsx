import React, { useState } from "react";
import axios from "axios"; // axios importálása
import Header from "./Header";
import Footer from "./Footer";
import { Link } from "react-router-dom";

const faqs = [
    { question: "Hol tudok foglalni?", answer: <>Minden szoba oldalán van foglalási lehetőség. <Link to="/tracks">Foglalj itt!</Link></> },
    { question: "Mennyibe kerül?", answer: <>Az árakat megtalálod az <Link to="/prices">Árak</Link> oldalon.</> },
    { question: "Milyen ruhába menjünk?", answer: "Játszós, koszolható ruházat ajánlott. Természetesen nem tesszük tönkre a ruházatodat, de mindenre is számolni kell!" },
    { question: "Hány éves kortól ajánljuk?", answer: <>Van olyan pályánk, amit már 12 éves kortól bátran ajánlunk, van, amit inkább 16, és vannak felnőtt pályáink – itt a díszlet lehet kicsit véres és ijesztő. Erről érdeklődj a <Link to="/contact">Kapcsolat</Link> oldalon!</> },
    { question: "Hány fő játszhat egyszerre?", answer: "A pályáink általában 2-6 fős csapatokat fogadnak, de van néhány nagyobb pálya is, amely akár 8 fő számára is elérhető." },
    { question: "Mi történik, ha nem sikerül kijutni?", answer: "Semmi gond! A játékmester beenged, és segít kijutni. A játékélmény így is teljes!" },
    { question: "Van fényképezési lehetőség?", answer: "Igen! A sikeresen kijutott csapatokat le is fotózzuk, és a képeket feltöltjük a Galéria oldalra." },
    { question: "Van lehetőség ajándékutalvány vásárlására?", answer: "Igen! Ajándékutalványainkat a recepción vagy online is megvásárolhatod." },
];

const FAQ = () => {
    const [question, setQuestion] = useState("");
    const [showPopup, setShowPopup] = useState(false);
    const [error, setError] = useState(""); // Hibaüzenet állapot

    const handleSubmit = async (e) => { // async hozzáadása
        e.preventDefault();
        if (question.trim() === "") return;

        try {
            await axios.post("http://localhost:5000/api/questions", { // API végpont
                question,
            });

            setShowPopup(true);
            setTimeout(() => setShowPopup(false), 3000);
            setQuestion("");
            setError(""); // Hibaüzenet törlése
        } catch (error) {
            console.error("Hiba a kérdés küldése során:", error);
            setError("Hiba történt a kérdés küldése során. Kérjük, próbálja újra."); // Hibaüzenet beállítása
        }
    };

    return (
        <>
            <Header />
            <main className="faq-container">
                <h1>Gyakran Ismételt Kérdések</h1>
                <p>Itt megtalálod a leggyakrabban felmerülő kérdéseket és válaszokat.</p>

                {/* Kérdés beküldő űrlap */}
                <form className="faq-form" onSubmit={handleSubmit}>
                    <textarea
                        placeholder="Írd le a kérdésed..."
                        value={question}
                        onChange={(e) => setQuestion(e.target.value)}
                        required
                    />
                    <button type="submit">Kérdés beküldése</button>
                </form>

                {/* Felugró ablak a beküldés után */}
                {showPopup && (
                    <div className="popup">
                        <p>A kérdésedet megkaptuk! Választ 48 órán belül itt olvashatod.</p>
                    </div>
                )}
                {error && <p className="error">{error}</p>} {/* Hibaüzenet megjelenítése */}

                {/* GYIK lista */}
                <div className="faq-list">
                    {faqs.map((faq, index) => (
                        <div key={index} className="faq-item">
                            <h3>{faq.question}</h3>
                            <p>{faq.answer}</p>
                        </div>
                    ))}
                </div>
            </main>
            <Footer />
        </>
    );
};

export default FAQ;