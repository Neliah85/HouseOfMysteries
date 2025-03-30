import React from "react";
import Header from "./Header";
import Footer from "./Footer";

const Prices = () => {
    return (
        <>
            <Header />
            <main className="prices-container">
                <h1>Áraink</h1>
                <p>Az alábbi árak tartalmazzák a teljes szabadulószoba élményt, csapatod létszámától függően.</p>

                <div className="table-responsive"> {/* Reszponzív táblázat */}
                    <table className="prices-table">
                        <thead>
                            <tr>
                                <th scope="col">Csapatlétszám</th>
                                <th scope="col">Felnőtt ár (Ft/fő)</th>
                                <th scope="col">Diák ár (Ft/fő)</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>2-4 fő</td>
                                <td>3000 Ft</td>
                                <td>2800 Ft</td>
                            </tr>
                            <tr>
                                <td>5 fő</td>
                                <td>2800 Ft</td>
                                <td>2600 Ft</td>
                            </tr>
                            <tr>
                                <td>6 fő</td>
                                <td>2600 Ft</td>
                                <td>2300 Ft</td>
                            </tr>
                            <tr>
                                <td>6 fő felett</td>
                                <td>2300 Ft</td>
                                <td>2000 Ft</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <p className="small-text">A feltüntetett árak forintban értendőek és az Áfát tartalmazzák!</p>
                <p className="group-pricing">
                    <strong>Nagy csoportok, cégek esetén, kérj egyedi ajánlatot a <a href="/contact">Kapcsolat</a> oldalon!</strong>
                </p>
                <p>
                    <strong>Máris foglalnál? Ezt könnyen megteheted <a href="/tracks">Itt</a>!</strong>
                </p>
            </main>
            <Footer />
        </>
    );
};

export default Prices;