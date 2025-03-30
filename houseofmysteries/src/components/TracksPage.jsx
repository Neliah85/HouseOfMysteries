import React from "react";
import Header from "./Header";
import Footer from "./Footer";
import { Link } from "react-router-dom";

import room1 from "../assets/images/room1.jpg";
import room2 from "../assets/images/room2.jpg";
import room3 from "../assets/images/room3.jpg";
import room4 from "../assets/images/room4.jpg";
import room5 from "../assets/images/room5.jpg";
import room6 from "../assets/images/room6.jpg";
import room7 from "../assets/images/room7.jpg";
import room8 from "../assets/images/room8.jpg";
import room9 from "../assets/images/room9.jpg";


const tracks = [
    { id: "room1", name: "Menekülés az iskolából", img: room1, description: "Képzeld el, hogy a tanórák végeztével egy különös varázslat miatt az iskola minden ajtaja bezáródik, és egyetlen kivezető utat sem találsz. A feladatod, hogy felfedezd az iskola rejtett sarkaiban megbúvó titkokat és megoldj különböző rejtvényeket, hogy megtaláld a szabadulás kulcsát. Minden terem saját kihívásokkal vár, és a tanári szekrények, könyvtárak, valamint a régi iskolai iratok között rejtett nyomokat kereshetsz. Az iskola történetei és legendái segítenek vagy éppen hátráltatnak a megoldásban. A csapdák és meglepetések révén egyre közelebb kerülsz a kijutáshoz, de a végső próba még hátravan. Készen állsz, hogy megismerd az iskola legnagyobb titkait és kijuss az elzárt épületből?" },
    { id: "room2", name: "A pedellus bosszúja", img: room2, description: "A pedellus, a hírhedt iskolai felügyelő, akinek nevével még ma is rettegetnek, visszatért, hogy megkeserítse az életedet. Elcsíptél egy elfeledett feljegyzést, amely rávilágít arra, hogy bosszúja nem ért véget – sőt, most kezdődik igazán. A szobában minden egyes tárgy és rejtvény a pedellus mérgét és hatalmát hirdeti. Meg kell oldanod a titokzatos és zavarba ejtő feladványokat, hogy megtudd, milyen csapdákat állított eléd. Az idő szorít, mert a pedellus hatalma egyre nő, és a szobában rejlő titkok egyre nehezebbé teszik a menekülést. Készen állsz szembenézni a pedellus haragjával és megtalálni a kijáratot, mielőtt túl késő lenne?" },
    { id: "room3", name: "A tanári titkai", img: room3, description: "Belépve a tanári szobába, egy régi, elfeledett múlt köszön vissza rád. A tanárok sosem beszéltek a szoba rejtett titkairól, de most rajtad a sor, hogy felderítsd azokat. Minden asztalon, fiókban és könyvben új nyomok és kódok várnak felfedezésre. A titkos üzenetek és rejtélyes könyvek nyomozása során ráébredsz, hogy a tanárok egy évszázados titkot őriztek. Ahogy egyre mélyebbre ásol, úgy egyre több történelmi titok és személyes titok kerül felszínre. Vajon képes leszel megfejteni a tanári szoba rejtélyeit és megtalálni a titokzatos kódot, ami az ajtó nyitja?" },
    { id: "room4", name: "A takarítónő visszanéz", img: room4, description: "A takarítónő, aki sok éven keresztül dolgozott az épületben, visszatér, hogy megvédje titkait. A szobát, amit először csak egy egyszerű raktárnak gondoltál, rejtélyes eszközökkel és furcsa nyomokkal töltötte meg. Az ősi feljegyzések és a régi takarítószerek körül bonyolult rejtvények várnak rád. A helyszín tele van furcsa utalásokkal és nyomokkal, amelyek a takarítónő titkait próbálják elrejteni. A szobában eltöltött idő alatt megtudod, hogy a takarítónő élete nem volt olyan egyszerű, mint ahogy azt elsőre gondoltad. Meg tudod fejteni a titkokat és megtalálni a kiutat, vagy a takarítónő bosszúja végleg megbénít?" },
    { id: "room5", name: "Szabadulás Kódja", img: room5, description: "A Szabadulás Kódja egy titokzatos laboratórium ajtaját rejti, ahol minden egyes lépés egy újabb kihívást jelent. A laboratórium minden sarkában és berendezésében új feladványokat találsz, amelyek a szabadulás kulcsait rejtik. A falakon lévő rejtett kódok és a bonyolult gépek segítségével próbálhatod megfejteni a zárakat. A szobában található nyomok és kódok olyan feladatokat rejtenek, amelyek csak a legokosabbakat és legkitartóbbakat várják. Az idő ellened dolgozik, és a kódok megfejtésével próbálhatod meg elérni a végső célodat: a szabadulást. Képes leszel kibogozni a titokzatos kódokat, mielőtt az idő lejár?" },
    { id: "room6", name: "Időcsapda", img: room6, description: "Az Időcsapda szobában egy időutazás vár rád, ahol a múlt és a jövő határvonalán kell navigálnod. A szobában minden egyes elem – a régi órák, a futurisztikus eszközök és a rejtett mechanizmusok – egy új időszakot képvisel. A célod, hogy megtaláld az időcsapda rejtett kulcsát, miközben különböző időszakok rejtélyeivel kell szembenézned. Minden lépés egy új kihívást jelent, ahogy próbálod helyreállítani az idő vonalát és megtalálni a kiutat. Az időérzékedet próbára téve kell megoldanod a feladványokat és kikerülnöd az időcsapda csapdáit. Vajon sikerül kijutnod a múlt és a jövő labirintusából?" },
    { id: "room7", name: "KódX Szoba", img: room7, description: "A KódX Szoba egy technológiai labirintus, ahol minden digitális eszköz és kód egy újabb rejtélyt rejt. A szoba tele van fejlett technológiai eszközökkel, rejtett kódokkal és mechanikus rendszerekkel, amelyek megoldásához szükség van az intelligenciádra és kreativitásodra. A feladatod, hogy navigálj a digitális világban, és megfejtsd a rejtett kódokat, amelyek a szoba különböző pontjain elhelyezkednek. Minden egyes kód egy újabb lépést jelent a szabadulás felé, miközben technológiai csapdákkal és kihívásokkal találkozol. Meg tudod oldani a KódX Szoba titkait és kijutni az elektronikus labirintusból?" },
    { id: "room8", name: "Kalandok Kamrája", img: room8, description: "A Kalandok Kamrája egy mesés helyszín, ahol a fantázia és a kaland keveredik. Az ódon kamrában egy sor izgalmas feladvány és rejtett kincs vár felfedezésre. A szoba minden sarkában új kihívásokkal és varázslatos elemekkel találkozol, amelyek próbára teszik ügyességedet és bátorságodat. Az ősi könyvek, rejtett nyomok és varázslatos eszközök segítségével kell megtalálnod az utat a kijárat felé. A kaland során számos érdekes karakterrel és történettel találkozhatsz, amelyek további nyomokat adhatnak a célod eléréséhez. Készen állsz a legnagyobb kalandra és a kincs megtalálására?" },
    { id: "room9", name: "Titkok Labirintusa", img: room9, description: "A Titkok Labirintusa egy komplex és zavarba ejtő labirintus, ahol minden fordulat új titkokat és kihívásokat rejt. Az útvesztő falai között rejtett üzenetek, rejtélyes szimbólumok és bonyolult feladványok várnak rád. Ahogy egyre mélyebbre hatolsz a labirintusban, úgy egyre bonyolultabbá válnak a feladatok, és a labirintus rejtett titkai is fokozatosan feltárulnak. A célod, hogy megtaláld a kijáratot, miközben megoldod a titokzatos rejtvényeket és navigálsz a labirintus szűk folyosóin. Vajon sikerül kijutnod a Titkok Labirintusából és megfejteni a legnagyobb rejtélyeket?" }
];

const TracksPage = () => {
    return (
        <>
            <Header />
            <main>
                <h1>Pályák és Foglalás</h1>
                <p>Válaszd ki a számodra legizgalmasabb pályát, és kezdődjön a kaland!</p>

                <div className="tracks-list">
                    {tracks.map((track) => (
                        <div key={track.id} className="track-item">
                            <Link to={`/booking/${track.id}`}>
                                <img src={track.img} alt={track.name} width="320" height="233" />
                            </Link>
                            <h2>{track.name}</h2>
                            <p>{track.description}</p>
                            <Link to={`/booking/${track.id}`} className="button">
                                Foglalás
                            </Link>
                        </div>
                    ))}
                </div>
            </main>
            <Footer />
        </>
    );
};

export default TracksPage;
