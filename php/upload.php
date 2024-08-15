<?php
$target_dir = "photos/";
$target_file = $target_dir . basename($_FILES["fileToUpload"]["name"]);
$uploadOk = 1;
$imageFileType = strtolower(pathinfo($target_file, PATHINFO_EXTENSION));

// Ellenőrizd, hogy kép-e a fájl
$check = getimagesize($_FILES["fileToUpload"]["tmp_name"]);
if($check !== false) {
    $uploadOk = 1;
} else {
    echo "A fájl nem kép.";
    $uploadOk = 0;
}

// Ellenőrizd, hogy a fájl már létezik-e
if (file_exists($target_file)) {
    echo "Sajnáljuk, de a fájl már létezik.";
    $uploadOk = 0;
}

// Ellenőrizd a fájl méretét
if ($_FILES["fileToUpload"]["size"] > 500000) {
    echo "Sajnáljuk, de a fájl túl nagy.";
    $uploadOk = 0;
}

// Engedélyezett fájlformátumok
if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg" && $imageFileType != "gif" ) {
    echo "Csak JPG, JPEG, PNG & GIF fájlok engedélyezettek.";
    $uploadOk = 0;
}

// Ha minden rendben van, próbáld meg feltölteni a fájlt
if ($uploadOk == 1) {
    if (move_uploaded_file($_FILES["fileToUpload"]["tmp_name"], $target_file)) {
        echo "A fájl ". htmlspecialchars( basename( $_FILES["fileToUpload"]["name"])). " feltöltésre került.";
    } else {
        echo "Sajnáljuk, de hiba történt a fájl feltöltése közben.";
    }
}
?>
