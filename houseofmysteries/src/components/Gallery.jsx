import React, { useState, useEffect } from "react";
import Header from "./Header";
import Footer from "./Footer";

const Gallery = () => {
    const [images, setImages] = useState([]);
    const [previewImage, setPreviewImage] = useState(null);

    useEffect(() => {
        const storedImages = JSON.parse(localStorage.getItem("galleryImages")) || [];
        setImages(storedImages);
    }, []);

    const handleImageUpload = (event) => {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onloadend = () => {
                setPreviewImage(reader.result);
            };
            reader.readAsDataURL(file);
        }
    };

    const handleConfirmUpload = () => {
        if (previewImage) {
            const newImages = [...images, previewImage];
            setImages(newImages);
            localStorage.setItem("galleryImages", JSON.stringify(newImages));
            setPreviewImage(null);
        }
    };

    const handleCancelUpload = () => {
        setPreviewImage(null);
    };

    const handleDeleteImage = (index) => {
        const newImages = images.filter((_, i) => i !== index);
        setImages(newImages);
        localStorage.setItem("galleryImages", JSON.stringify(newImages));
    };

    return (
        <>
            <Header />
            <main className="gallery-container">
                <h1>Galéria</h1>
                <p>Itt láthatod a sikeresen kiszabadult csapatokat!</p>

                <label className="upload-button">
                    📷 Kép feltöltése
                    <input type="file" accept="image/*" onChange={handleImageUpload} />
                </label>

                {previewImage && (
                    <div className="preview-container">
                        <img src={previewImage} alt="Kép előnézet" className="preview-image" />
                        <button onClick={handleConfirmUpload}>Feltöltés</button>
                        <button onClick={handleCancelUpload}>Mégse</button>
                    </div>
                )}

                <div className="gallery-grid">
                    {images.map((image, index) => (
                        <div key={index} className="gallery-item">
                            <img src={image} alt={`Feltöltött kép ${index + 1}`} />
                            <button onClick={() => handleDeleteImage(index)}>Törlés</button>
                        </div>
                    ))}
                </div>
            </main>
            <Footer />
        </>
    );
};

export default Gallery;