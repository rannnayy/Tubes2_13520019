# Folder Crawler by FilePedia
Tugas Besar 2 IF2211 Strategi Algoritma

## Deskripsi Program
Folder Crawler adalah program untuk pencarian file pada _directory_ tertentu. Program ini melakukan pencarian dengan dua pendekatan yang berbeda, yaitu Breadth First Search (BFS) dan Depth First Search (DFS). Program Folder Crawler dikembangkan menggunakan WinForms dengan bahasa pemrograman C#. 

## Requirement Program dan Instalasi Module
Folder Crawler menggunakan WinForms dan dalam bahasa C# untuk pengembangannya. Untuk menjalankan program Folder Crawler, pertama lakukan _clone_ pada _repository_ ini dengan menggunakan terminal, command prompt, atau program CLI lainnya. 

	git clone https://github.com/rannnayy/Tubes2_13520019.git
	cd Tubes2_13520019
		
Setelah berada di folder Tubes2_13520019,  terdapat file bernama **stima-filePedia.sln** dalam folder **src** yang dapat dibuka dengan perangkat lunak **Visual Studio** (direkomendasikan menggunakan Visual Studio versi terbaru atau versi saat pembuatan program yaitu Visual Studio 2022). 

Program Folder Crawler memerlukan pustaka bernama MSAGL (Microsoft Automatic Graph Layout). Pustaka ini telah dilampirkan pada repository sehingga tidak diperlukan instalasi pada MSAGL.

## Langkah Compile Program
### Development Mode
Untuk membuka program dalam mode pengembangan, buka **stima-filePedia.sln** menggunakan Visual Studio. Pada menu bar, klik **Debug > Start Debugging**. Program akan berjalan dengan mode pengembangan serta melakukan logging pada Debug Output di Visual Studio.

### Production Mode
Program akhir akan dihasilkan melalui langkah-langkah sebagai berikut.
1. Buka stima-filePedia.sln dengan menggunakan Visual Studio.
2. Pada menu bar di Visual Studio, klik **Build > Build stima-filePedia**.
3. Pada folder Tubes2_13520019, buka folder bin, lalu ke folder debug.
4. Program yang sudah di-compile dapat dijalankan dengan melakukan _run_ pada **stima-filePedia.exe**.
5. Jika program ingin dipindahkan ke tempat lain, lakukan juga pemindahan bersamaan untuk semua file di folder tersebut seperti file AutomaticGraphLayout.dll dan lain sebagainya. Hal ini dilakukan agar program dapat berjalan dengan pustaka yang dibutuhkan program tersebut.

## Cara Penggunaan Program
Program yang berhasil di-_compile_ dapat dijalankan. Cara menggunakan program Folder Crawler adalah sebagai berikut.
1. Lakukan _run_ pada program **stima-filePedia.exe**.
2. Klik **Choose Folder** untuk menentukan folder basis pencarian file.
3. Ketik nama file lengkap dengan ekstensinya pada area **File Name**.
4. Jika perlu mencari semua file dengan nama yang sama pada input File Name, centang **Find All Occurences**. Jika tidak dicentang, pencarian akan berhenti saat file pertama ditemukan.
5. Pilih algoritma yang digunakan, **Breadth First Search** atau **Depth First Search**.
6. Klik tombol **Search!**.
7. Program akan berjalan sesuai dengan algoritma yang dipilih. Selain itu, program akan menampilkan visualisasi dari _progress_ algoritma yang dijalankan (BFS/DFS). Program juga akan memberikan waktu eksekusi algoritma hingga pencarian berakhir.
8. Pencarian telah selesai. Jika ingin melakukan pencarian file pada folder lain, ulangi dari langkah ke-2.

## Identitas Pembuat
|Nama|NIM|
|--|--|
|Maharani Ayu Putri Irawan|13520019|
|Rizky Ramadhana P. K.|13520151|
|Vito Ghifari|13520153|
