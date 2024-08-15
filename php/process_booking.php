<?php
// Adatbázis kapcsolat
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "booking_db";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Kapcsolódási hiba: " . $conn->connect_error);
}

// Adatok begyűjtése
$track = $_POST['track'];
$date = $_POST['date'];
$time_slot = $_POST['time_slot'];
$name = $_POST['name'];
$phone = $_POST['phone'];
$participants = $_POST['participants'];
$payment = $_POST['payment'];

// Adatok tárolása az adatbázisban
$sql = "INSERT INTO bookings (track, date, time_slot, name, phone, participants, payment) 
        VALUES ('$track', '$date', '$time_slot', '$name', '$phone', '$participants', '$payment')";

if ($conn->query($sql) === TRUE) {
    // E-mail küldése
    $to = "ceg@example.com";
    $subject = "Új foglalás érkezett";
    $message = "Új foglalás érkezett:\n\nPálya: $track\nDátum: $date\nIdőpont: $time_slot\nNév: $name\nTelefonszám: $phone\nRésztvevők száma: $participants\nFizetési mód: $payment";
    $headers = "From: noreply@example.com";

    mail($to, $subject, $message, $headers);

    echo "Foglalás sikeresen elküldve!";
} else {
    echo "Hiba történt a foglalás során: " . $conn->error;
}

$conn->close();
?>
