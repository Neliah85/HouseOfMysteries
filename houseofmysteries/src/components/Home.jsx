import React, { useState, useEffect } from "react";
import Header from "./Header";
import Footer from "./Footer";
import { useNavigate } from 'react-router-dom';


import room1 from "../assets/images/room1.jpg";
import room2 from "../assets/images/room2.jpg";
import room3 from "../assets/images/room3.jpg";
import room4 from "../assets/images/room4.jpg";
import room5 from "../assets/images/room5.jpg";
import room6 from "../assets/images/room6.jpg";
import room7 from "../assets/images/room7.jpg";
import room8 from "../assets/images/room8.jpg";
import room9 from "../assets/images/room9.jpg";


const slides = [
    { id: 1, text: "🎉 Új pálya: A pedellus bosszúja", image: "/images/slide1.jpg" },
    { id: 2, text: "🔥 Februári akció: 10% kedvezmény hétköznapokon!", image: "/images/slide2.jpg" },
    { id: 3, text: "🕵️‍♂️ Tavaszi kalandok: Jön az új horror szabadulószoba!", image: "/images/slide3.jpg" },
];


const Home = () => {
    const navigate = useNavigate();
    const [currentSlide, setCurrentSlide] = useState(0);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            // Itt már nem szükséges a backend hívás a token ellenőrzésére
            // A token érvényességét a backend a védett útvonalakhoz való hozzáféréskor ellenőrzi
            console.log("Token a localStorage-ból:", token);
        } else {
            console.log("Nincs token a localStorage-ban.");
            // Ha nincs token, átirányíthatod a felhasználót a bejelentkezési oldalra
            // navigate('/login');
        }

        const interval = setInterval(() => {
            setCurrentSlide((prev) => (prev + 1) % slides.length);
        }, 5000);
        return () => clearInterval(interval);
    }, [navigate]);

    const rooms = [
        { id: "room1", name: "Menekülés az iskolából", img: room1 },
        { id: "room2", name: "A pedellus bosszúja", img: room2 },
        { id: "room3", name: "A tanári titkai", img: room3 },
        { id: "room4", name: "A takarítónő visszanéz", img: room4 },
        { id: "room5", name: "Szabadulás Kódja", img: room5 },
        { id: "room6", name: "Időcsapda", img: room6 },
        { id: "room7", name: "KódX Szoba", img: room7 },
        { id: "room8", name: "Kalandok Kamrája", img: room8 },
        { id: "room9", name: "Titkok Labirintusa", img: room9 }
    ];

    return (
        <>
            <Header />
            <main>

                {/* Slideshow beillesztése */}
                <div className="slideshow">
                    <img src={slides[currentSlide].image} alt="Slideshow" className="slide-image" />
                    <p className="slide-text">{slides[currentSlide].text}</p>
                </div>

                <h1>Üdvözlünk a Rejtélyek Házában – </h1>
                <h3>A Kalandok és Titkok Birodalmában!</h3>
                
                <p>Merülj el a kalandok világában, ahol minden rejtély új kihívásokat tartogat! Fedezd fel a legizgalmasabb szabadulószobákat, és tudd meg, képes vagy-e megoldani a rejtélyeket!</p>
                
                <div className="tracks-grid">
                    {rooms.map((room) => (
                        <figure key={room.id}>
                            <a href={`tracks/${room.id}`}>
                                <img src={room.img} alt={room.name} width="320" height="200" />
                            </a>
                            <figcaption>{room.name}</figcaption>
                        </figure>
                    ))}
                </div>

                <h2>Miért válaszd a mi szabadulószobánkat?</h2>
                <p>
                    Amikor szabadulószobát választasz, fontos, hogy a legjobb élményt kapd. Mi a Rejtélyek Háza szabadulószobánál ezt pontosan tudjuk, és ezért mindent megteszünk, hogy vendégeink számára felejthetetlen kalandot nyújtsunk. Nálunk nem csupán egy egyszerű játékot találsz, hanem egy teljesen új világba való belépést, ahol a kihívások és a szórakozás keveredik.
                </p>
                <p>
                    Pályáink mindegyike egyedi történettel és izgalmas feladványokkal vár, amelyek a legnagyobb figyelmet és kreativitást is megkövetelik. Legyen szó a régi iskolai rejtélyekről, egy modern technológiai kihívásról, vagy éppen egy misztikus kalandról, nálunk mindenki megtalálja a számára tökéletes pályát. Szakértő csapatunk gondoskodik arról, hogy minden apró részlet tökéletes legyen, és hogy a játékélmény maximálisan kielégítő legyen.
                </p>
                <p>
                    Szabadulószobáink nemcsak szórakoztatóak, hanem biztonságosak is. Folyamatosan figyelemmel kísérjük a játékmenetet, és segítséget nyújtunk, ha szükséges, így biztosítva, hogy a játék mindenki számára élvezetes és problémamentes legyen. A hangulatosan berendezett szobák, a kreatív feladványok és a különleges dizájn garantálják, hogy minden látogatás egy új kalandot jelent.
                </p>
                <p>Ne hagyd ki a lehetőséget, hogy részese legyél egy olyan élménynek, amely mind a kihívás, mind a szórakozás terén felülmúlja várakozásaidat. Látogass el hozzánk a Rejtélyek Házába, és tapasztald meg, miért vagyunk az egyik legkedveltebb szabadulószoba a városban. Itt az ideje, hogy megoldj egy rejtélyt, ami csak rád vár!</p>
                <p><strong>Tarts velünk, és fedezd fel a Rejtélyek Háza izgalmas világát – ahol a szórakozás és a kihívás határtalan!</strong></p>
            </main>
            <Footer />
        </>
    );
};

export default Home;
