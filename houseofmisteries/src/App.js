import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./components/Home";
import Booking from "./components/Booking";
import TracksPage from "./components/TracksPage";
import TrackPage from "./components/TrackPage";
import About from "./components/About";
import Prices from "./components/Prices";
import Reviews from "./components/Reviews";
import Gallery from "./components/Gallery";
import Faq from "./components/Faq";
import Contact from "./components/Contact";
import Login from "./components/Login";
import Register from "./components/Register";
import Admin from "./components/Admin";
import Profile from "./components/Profile";
import PrivacyPolicy from './components/Privacy';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/booking/:id" element={<Booking />} />
                <Route path="/tracks" element={<TracksPage />} />
                <Route path="/tracks/:id" element={<TrackPage />} /> 
                <Route path="/about" element={<About />} />
                <Route path="/prices" element={<Prices />} />
                <Route path="/reviews" element={<Reviews />} />
                <Route path="/gallery" element={<Gallery />} />
                <Route path="/booking" element={<Booking />} />
                <Route path="/faq" element={<Faq/>} />
                <Route path="/contact" element={<Contact/>} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/admin" element={<Admin />} />
                <Route path="/profile" element={<Profile />} />
                <Route path="/privacy" element={<PrivacyPolicy />} />
            </Routes>
        </Router>
    );
}

export default App;
