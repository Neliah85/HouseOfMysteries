<?php
// Példa adatbázis kapcsolat
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "booking_db";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Kapcsolódási hiba: " . $conn->connect_error);
}

// Választott pálya
$track = $_POST['track'];

// Jelenlegi dátum
$currentDate = date('Y-m-d');
$endDate = date('Y-m-d', strtotime('+30 days'));

// Foglalt időpontok lekérdezése
$sql = "SELECT date, time_slot FROM bookings WHERE track = '$track' AND date BETWEEN '$currentDate' AND '$endDate'";
$result = $conn->query($sql);

$bookedSlots = [];
while ($row = $result->fetch_assoc()) {
    $bookedSlots[$row['date']][] = $row['time_slot'];
}

// Időpontok generálása
$timeSlots = [];
for ($i = 9; $i <= 21; $i++) {
    $timeSlots[] = "$i:00-" . ($i + 1) . ":00";
}

?>

<!DOCTYPE html>
<html lang="hu">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Naptár - Foglalás</title>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <header>
        <nav>
            <ul class="menu">
                <li><a href="index.html">Főoldal</a></li>
                <li><a href="tracks.html">Pályák és foglalás</a></li>
                <li><a href="prices.html">Árak</a></li>
                <li><a href="reviews.html">Vélemények</a></li>
                <li><a href="faq.html">Gyik</a></li>
                <li><a href="contact.html">Kapcsolat</a></li>
                <li><a href="gallery.html">Galéria</a></li>
            </ul>
        </nav>
    </header>

    <main>
        <section class="calendar-section">
            <h1>Foglalás a következő pályára: <?php echo htmlspecialchars($track); ?></h1>
            <form method="POST" action="/database/confirm_booking.php">
                <input type="hidden" name="track" value="<?php echo htmlspecialchars($track); ?>">

                <?php
                for ($date = strtotime($currentDate); $date <= strtotime($endDate); $date = strtotime('+1 day', $date)) {
                    $dateString = date('Y-m-d', $date);
                    echo "<h3>" . date('Y.m.d (l)', $date) . "</h3>";
                    echo "<ul>";
                    foreach ($timeSlots as $slot) {
                        if (!in_array($slot, $bookedSlots[$dateString] ?? [])) {
                            echo "<li><label><input type='radio' name='time_slot' value='$dateString|$slot' required> $slot</label></li>";
                        }
                    }
                    echo "</ul>";
                }
                ?>

                <button type="submit">Tovább a foglaláshoz</button>
            </form>
        </section>
    </main>

    <footer>
        <p>&copy; 2024 Szabadulószoba. Minden jog fenntartva.</p>
    </footer>
</body>
</html>
