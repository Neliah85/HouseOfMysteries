<?php
// Adatbázis kapcsolat beállításai
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "contact_form";

// Kapcsolódás az adatbázishoz
$conn = new mysqli($servername, $username, $password, $dbname);

// Kapcsolat ellenőrzése
if ($conn->connect_error) {
    die("Kapcsolódási hiba: " . $conn->connect_error);
}

// Adatok fogadása az űrlapból
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $name = $_POST['name'];
    $email = $_POST['email'];
    $phone = $_POST['phone'];
    $message = $_POST['message'];
    $consent = isset($_POST['consent']) ? 1 : 0;

    // SQL lekérdezés beszúrásra
    $sql = "INSERT INTO contacts (name, email, phone, message, consent) 
            VALUES ('$name', '$email', '$phone', '$message', '$consent')";

    if ($conn->query($sql) === TRUE) {
        echo "Az üzenet sikeresen elküldve.";
    } else {
        echo "Hiba történt: " . $sql . "<br>" . $conn->error;
    }
}

// Kapcsolat bezárása
$conn->close();
?>
