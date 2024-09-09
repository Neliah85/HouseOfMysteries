<?php
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

// Adatok ellenőrzése és mentése
$name = isset($_POST['name']) ? $_POST['name'] : '';
$email = isset($_POST['email']) ? $_POST['email'] : '';
$phone = isset($_POST['phone']) ? $_POST['phone'] : '';
$message = isset($_POST['message']) ? $_POST['message'] : '';

// SQL lekérdezés
$sql = "INSERT INTO contact_form (name, email, phone, message) VALUES ('$name', '$email', '$phone', '$message')";

if ($conn->query($sql) === TRUE) {
    echo "Üzenet elküldve!";
} else {
    echo "Hiba: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>
