<?php
$track = $_POST['track'];
list($date, $time_slot) = explode('|', $_POST['time_slot']);
?>

<!DOCTYPE html>
<html lang="hu">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Foglalás megerősítése - Szabadulószoba</title>
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
        <section class="confirm-booking-section">
            <h1>Foglalás megerősítése</h1>
            <p>Pálya: <?php echo htmlspecialchars($track); ?></p>
            <p>Dátum: <?php echo htmlspecialchars($date); ?></p>
            <p>Időpont: <?php echo htmlspecialchars($time_slot); ?></p>

            <form method="POST" action="process_booking.php">
                <input type="hidden" name="track" value="<?php echo htmlspecialchars($track); ?>">
                <input type="hidden" name="date" value="<?php echo htmlspecialchars($date); ?>">
                <input type="hidden" name="time_slot" value="<?php echo htmlspecialchars($time_slot); ?>">

                <label for="name">Név:</label>
                <input type="text" id="name" name="name" required>

                <label for="phone">Telefonszám:</label>
                <input type="tel" id="phone" name="phone" required>

                <label for="participants">Résztvevők száma:</label>
                <select id="participants" name="participants" required>
                    <option value="1">1 fő</option>
                    <option value="2">2 fő</option>
                    <option value="3">3 fő</option>
                    <option value="4">4 fő</option>
                    <option value="5">5 fő</option>
                    <option value="6">6 fő</option>
                </select>

                <label for="payment">Fizetési mód:</label>
                <select id="payment" name="payment" required>
                    <option value="cash">Helyszínen készpénz</option>
                    <option value="card">Helyszínen kártya</option>
                    <option value="bank_transfer">Előre utalás</option>
                    <option value="voucher">Ajándék utalvány</option>
                    <option value="szep_card">SZÉP Kártya</option>
                </select>

                <label><input type="checkbox" name="privacy" required> Elfogadom az adatkezelési feltételeket</label>

                <button type="submit">Foglalás elküldése</button>
            </form>
        </section>
    </main>

    <footer>
        <p>&copy; 2024 Szabadulószoba. Minden jog fenntartva.</p>
    </footer>
</body>
</html>
