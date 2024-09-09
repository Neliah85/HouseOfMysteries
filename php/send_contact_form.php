<?php
// Adatbázis kapcsolati beállítások
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "szabaduloszoba";

// Kapcsolat létrehozása
$conn = new mysqli($servername, $username, $password, $dbname);

// Kapcsolat ellenőrzése
if ($conn->connect_error) {
    die("Kapcsolat hiba: " . $conn->connect_error);
}

// Az űrlap adatai
$name = $_POST['name'];
$email = $_POST['email'];
$phone = $_POST['phone'];
$message = $_POST['message'];

// SQL lekérdezés
$sql = "INSERT INTO contact_form (name, email, phone, message)
        VALUES ('$name', '$email', '$phone', '$message')";

if ($conn->query($sql) === TRUE) {
    echo "Az üzenet sikeresen elküldve!";
} else {
    echo "Hiba: " . $sql . "<br>" . $conn->error;
}

// Kapcsolat bezárása
$conn->close();
?>
