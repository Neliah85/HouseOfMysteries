<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Űrlapadatok begyűjtése
    $name = htmlspecialchars($_POST['name']);
    $email = htmlspecialchars($_POST['email']);
    $phone = htmlspecialchars($_POST['phone']);
    $message = htmlspecialchars($_POST['message']);
    $consent = isset($_POST['consent']) ? 'Elfogadva' : 'Nem fogadva el';

    // Email küldése
    $to = "info@szabaduloszoba.hu";
    $subject = "Kapcsolatfelvétel: $name";
    $body = "Név: $name\nEmail: $email\nTelefonszám: $phone\nÜzenet:\n$message\n\nAdatkezelési feltételek: $consent";
    $headers = "From: $email";

    if (mail($to, $subject, $body, $headers)) {
        echo "Üzenet sikeresen elküldve!";
    } else {
        echo "Hiba történt az üzenet küldése közben.";
    }

    // Adatbázisba mentés
    $servername = "localhost";
    $username = "root";
    $password = "";
    $dbname = "contact_form_db";

    // Kapcsolódás az adatbázishoz
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Kapcsolat ellenőrzése
    if ($conn->connect_error) {
        die("Kapcsolódási hiba: " . $conn->connect_error);
    }

    $sql = "INSERT INTO contact_messages (name, email, phone, message, consent)
            VALUES ('$name', '$email', '$phone', '$message', '$consent')";

    if ($conn->query($sql) === TRUE) {
        echo "Adatok sikeresen mentve!";
    } else {
        echo "Hiba az adatok mentésekor: " . $conn->error;
    }

    $conn->close();
}
?>
