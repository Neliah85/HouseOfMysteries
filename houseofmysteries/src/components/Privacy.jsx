import React from "react";
import Header from "./Header";
import Footer from "./Footer";

const PrivacyPolicy = () => {
    return (
        <>
            <Header />
            <main className="privacy-policy-container">
                <h1>Adatvédelmi Szabályzat</h1>

                <p>
                    A Rejtélyek Háza szabadulószoba (BKÁJZSMT Kft., Miskolc, Palóczy László utca 3, 3525) elkötelezett a felhasználók személyes adatainak védelme iránt. Ez az adatvédelmi szabályzat tájékoztatást nyújt arról, hogy milyen adatokat gyűjtünk, hogyan használjuk fel azokat, és milyen jogok illetik meg Önt az adataival kapcsolatban.
                </p>

                <h2>Milyen adatokat gyűjtünk?</h2>
                <p>
                    A weboldalunkon keresztül a következő személyes adatokat gyűjthetjük:
                </p>
                <ul>
                    <li>Név és elérhetőségi adatok (e-mail cím, telefonszám) foglalás vagy kapcsolatfelvétel esetén.</li>
                    <li>IP cím és böngészési adatok a weboldal használatának elemzéséhez.</li>
                    <li>Sütik (cookie-k) segítségével gyűjtött adatok a felhasználói élmény javítása érdekében.</li>
                </ul>

                <h2>Hogyan használjuk fel az adatokat?</h2>
                <p>
                    A gyűjtött adatokat a következő célokra használjuk fel:
                </p>
                <ul>
                    <li>Foglalások kezelése és visszaigazolása.</li>
                    <li>Kapcsolatfelvétel és kérdések megválaszolása.</li>
                    <li>Weboldalunk fejlesztése és a felhasználói élmény javítása.</li>
                    <li>Marketing célokra, amennyiben ehhez hozzájárult.</li>
                </ul>

                <h2>Adatok védelme</h2>
                <p>
                    Minden szükséges technikai és szervezési intézkedést megteszünk az Ön személyes adatainak védelme érdekében. Az adatokat biztonságos szervereken tároljuk, és gondoskodunk arról, hogy azokhoz csak a szükséges személyek férjenek hozzá.
                </p>

                <h2>Sütik (cookie-k)</h2>
                <p>
                    Weboldalunk sütiket használ a felhasználói élmény javítása érdekében. A sütik kis szöveges fájlok, amelyeket a böngészője tárol. A sütik segítségével információkat gyűjtünk a weboldal használatáról, például a látogatott oldalakról és a böngészési szokásokról.
                </p>

                <h2>Harmadik felekkel való adatmegosztás</h2>
                <p>
                    Személyes adatait harmadik felekkel csak akkor osztjuk meg, ha az szükséges a szolgáltatásaink nyújtásához (pl. foglalási rendszerek), vagy ha jogszabály kötelez rá.
                </p>

                <h2>Felhasználói jogok</h2>
                <p>
                    Önnek jogában áll:
                </p>
                <ul>
                    <li>Hozzáférés kérni a személyes adataihoz.</li>
                    <li>Helyesbíteni a pontatlan adatokat.</li>
                    <li>Törölni a személyes adatait.</li>
                    <li>Korlátozni az adatkezelést.</li>
                    <li>Tiltakozni az adatkezelés ellen.</li>
                    <li>Adathordozhatóságot kérni.</li>
                </ul>

                <h2>Kapcsolattartási adatok</h2>
                <p>
                    Amennyiben kérdése van az adatvédelmi szabályzatunkkal kapcsolatban, kérjük, vegye fel velünk a kapcsolatot:
                </p>
                <p><strong>Cégnév:</strong> BKÁJZSMT Kft.</p>
                <p><strong>Cím:</strong> Miskolc, Palóczy László utca 3, 3525</p>
                <p><strong>Telefonszám:</strong> <a href="tel:+36123456789">+36 12 345 6789</a></p>
                <p><strong>Email:</strong> <a href="mailto:rejtelyekhaza@gmail.com">rejtelyekhaza@gmail.com</a></p>

            </main>
            <Footer />
        </>
    );
};

export default PrivacyPolicy;