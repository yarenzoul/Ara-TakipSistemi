using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ARACTAKİP
{
    internal class Program
    {
        static void Main(string[] args)
        {


            List<string> Sofor = new List<string>();
            List<string> Arac = new List<string>();
            List<string> Gorev = new List<string>();

            string kullaniciAdi, sifre, sifreBilgi, hasliSifre;
            do
            {

                if (!File.Exists("ş1.yo"))
                {
                    using (StreamWriter sw = new StreamWriter("ş1.yo", true, Encoding.Default))
                    {
                        Console.Write("Kullanıcı Adı:");
                        kullaniciAdi = Console.ReadLine();
                        Console.Write("Şifre:");
                        sifre = MaskedPasswordInput();
                        sifreBilgi = kullaniciAdi + sifre + "GerçekŞifre";
                        hasliSifre = MD5Sifrele(sifreBilgi);

                        sw.WriteLine(hasliSifre);
                    }
                }

                Console.Write("Kullanıcı Adı:");
                kullaniciAdi = Console.ReadLine();
                Console.Write("Şifre:");
                sifre = MaskedPasswordInput();

                sifreBilgi = kullaniciAdi + sifre + "GerçekŞifre";
                hasliSifre = MD5Sifrele(sifreBilgi);

                bool kullaniciBulundu = false;
                using (StreamReader sr2 = new StreamReader("ş1.yo", Encoding.Default))
                {
                    string okunanş;
                    while ((okunanş = sr2.ReadLine()) != null)
                    {
                        if (okunanş == hasliSifre)
                        {
                            kullaniciBulundu = true;
                            break;
                        }
                    }
                }

                if (kullaniciBulundu)
                {
                    Console.WriteLine("Kullanıcı Girişi Başarılı.....\n Menü için bir tuşa basın.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.WriteLine("Kullanıcı Adı veya Şifre hatalı. Lütfen tekrar deneyiniz...");
                    Console.Write("Yeni bir hesap oluşturmak ister misiniz? (E/H): ");
                    string cevap = Console.ReadLine().ToUpper();
                    if (cevap == "E")
                    {
                        Console.Write("Yeni Kullanıcı Adı:");
                        kullaniciAdi = Console.ReadLine();
                        Console.Write("Yeni Şifre:");
                        sifre = MaskedPasswordInput();

                        sifreBilgi = kullaniciAdi + sifre + "GerçekŞifre";
                        hasliSifre = MD5Sifrele(sifreBilgi);

                        using (StreamWriter sw = new StreamWriter("ş1.yo", true, Encoding.Default))
                        {
                            sw.WriteLine(hasliSifre);
                            Console.WriteLine("Yeni hesap oluşturuldu. Lütfen tekrar giriş yapınız.");
                        }
                    }
                }
            } while (true);



            string MaskedPasswordInput()
            {
                string password = "";
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(true);


                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password.Remove(password.Length - 1);
                            Console.Write("\b \b");
                        }
                    }
                }
                while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();
                return password;
            }





            Console.WriteLine(" ARAÇ TAKİP SİSTEMİNE HOŞGELDİNİZ !");
            Console.WriteLine("*********** ANA MENÜ ***********");

            while (true)
            {
                Console.WriteLine("(S)-Şoför");
                Console.WriteLine("(A)-Araç");
                Console.WriteLine("(G)-Görev");
                Console.WriteLine("(Ç)-Çıkıs");
                Console.WriteLine("Seçiminiz:");
                string secim = Console.ReadLine().ToUpper();

                if (secim == "S")
                {
                    if (File.Exists("Sofor.ac") == false)
                    {
                        StreamWriter sw = new StreamWriter("Sofor.ac", true, Encoding.Default);
                        sw.Close();
                    }
                    StreamReader sr = new StreamReader("Sofor.ac", Encoding.Default);
                    string okunan;
                    Console.WriteLine("Kayıtlı Şoför Bilgileri");
                    Console.WriteLine("TC No\t\t|Ad\t|Soyad\t|Görevi");
                    Sofor.Clear();
                    while ((okunan = sr.ReadLine()) != null)
                    {
                        Sofor.Add(okunan);
                        string[] parca = okunan.Split('#');
                        Console.WriteLine(parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t");
                    }
                    sr.Close();

                    while (true)
                    {

                        Console.WriteLine("~Şoför Tablosu İşlemleri");
                        Console.WriteLine("1-Ekleme\r\n 2-Silme\r\n 3-Güncelleme\r\n 4-Tümünü Silme\r\n 5-Araya Ekle\r\n 6-Bul\r\n 7-Sırala\r\n 8-Listeleme\r\n 9-Ana Menüye Dön\r\n 0-Çıkış");
                        Console.WriteLine("Seçiminiz:");
                        secim = Console.ReadLine().ToUpper();


                        if (secim == "1")
                        {
                            string tcNo, ad, soyad, gorevi, tekrarSofor;
                            Console.WriteLine("---EKLEME İŞLEMLERİ---");
                            do
                            {

                                //TC NO

                                while (true)
                                {
                                    Console.WriteLine("TC No:");
                                    tcNo = Console.ReadLine();
                                    if (tcNo.Length == 11)
                                    {
                                        if (tamamiRakamMi(tcNo) == false)
                                            Console.WriteLine("TC No sadece rakamdan oluşmak zorunda");
                                        else
                                        {
                                            bool var = false;
                                            foreach (var it in Sofor)
                                            {
                                                string[] p = it.Split('#');
                                                if (tcNo == p[0])
                                                {
                                                    var = true;
                                                    break;
                                                }

                                            }
                                            if (var == true)
                                            {
                                                Console.WriteLine("Bu TC No zaten kayıtlı");
                                            }
                                            else
                                                break;
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("TC No 11 karakterden oluşmak zorunda!");
                                    }
                                }





                                //AD

                                while (true)
                                {
                                    Console.WriteLine("Adı:");
                                    ad = Console.ReadLine().ToUpper();
                                    if (ad.Length <= 2 || ad.Length >= 10)
                                        Console.WriteLine("ad 2 karakterden az 10 dan büyük olamaz!");
                                    else

                                        if (tamamiKarakterMi(ad) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //SOYAD

                                while (true)
                                {
                                    Console.WriteLine("Soyadı:");
                                    soyad = Console.ReadLine().ToUpper();
                                    if (soyad.Length <= 2 || soyad.Length > 15)
                                        Console.WriteLine("Soyad 2 karakterden az olamaz");
                                    else
                                         if (tamamiKarakterMi(soyad) == false)
                                    {
                                        Console.WriteLine(" Soyadı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //GÖREVİ

                                while (true)
                                {
                                    Console.WriteLine("Görevi:");
                                    gorevi = Console.ReadLine().ToUpper();
                                    if (gorevi.Length <= 9 || gorevi.Length >= 30)
                                        Console.WriteLine("Görevi en az 10 en fazla 30 karakter olmak zorunda!");
                                    else
                                         if (tamamiKarakterMi(gorevi) == false)
                                    {
                                        Console.WriteLine("Görevi sadece metinsel olabilir.");
                                    }
                                    else
                                        break;

                                }
                                string bilgi = tcNo + "#" + ad + "#" + soyad + "#" + gorevi;
                                Sofor.Add(bilgi);
                                StreamWriter sw = new StreamWriter("Sofor.ac", true, Encoding.Default);
                                sw.WriteLine(bilgi);
                                sw.Close();

                                Console.WriteLine("Şoför bilgileri eklendi, yeni eklemek istiyor musunuz?(E-H)");
                                tekrarSofor = Console.ReadLine().ToUpper();
                            } while (tekrarSofor == "E");
                        }
                        else if (secim == "2")
                        {
                            string tekrarSofor;
                            do
                            {


                                Console.WriteLine("---SİLME İŞLEMLERİ---");
                                int sayac = 1;
                                foreach (var item in Sofor)
                                {
                                    string[] parca = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t");
                                }
                                Console.WriteLine("Silmek istediğiniz satır numarasını giriniz:");
                                int sira = Convert.ToInt32(Console.ReadLine());
                                Sofor.RemoveAt(sira - 1);


                                StreamWriter sw = new StreamWriter("Sofor.ac", false, Encoding.Default);
                                foreach (var item in Sofor)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();

                                Console.WriteLine("Şoförünüz Silindi, yine silmek istiyor musunuz?(E-H)");
                                tekrarSofor = Console.ReadLine().ToUpper();
                            } while (tekrarSofor == "E");

                        }
                        else if (secim == "3")
                        {
                            string tekrarSofor;
                            do
                            {
                                Console.WriteLine("---ŞOFÖR GÜNCELLEME İŞLEMLERİ---");
                                Console.WriteLine("Dosyada Kayıtlı Şoförler");
                                Console.WriteLine("TC No\t\t|Ad\t|Soyad\t\t|Görevi");
                                int sayac = 1;
                                foreach (var item in Sofor)
                                {
                                    string[] parcaa = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parcaa[0] + "\t|" + parcaa[1] + "\t|" + parcaa[2] + "\t|" + parcaa[3] + "\t");
                                }
                                int sira;
                                while (true)
                                {
                                    Console.WriteLine("Güncellenecek istediğiniz satır numarasını giriniz:");
                                    if (int.TryParse(Console.ReadLine(), out sira) == false)
                                        Console.WriteLine("Sıra Numarası Düzgün Girilmemiş!\nTekrar...");
                                    else
                                    {
                                        if (sira < 1 || sira > sayac - 1)
                                        {
                                            Console.WriteLine("Olmayan Sıra Numarası Girilmiş!\nTekrar...");
                                        }
                                        else
                                            break;
                                    }
                                }
                                string tcNo, ad, soyad, gorevi;
                                sira = sira - 1;
                                string[] parca = Sofor[sira].Split('#');
                                //TC NO
                                while (true)
                                {
                                    Console.WriteLine("TC No:" + parca[0] + "\n Yeni Şoför Tc Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Şofor TC:");
                                    tcNo = Console.ReadLine();
                                    if (tcNo.Length == 0)
                                    {
                                        tcNo = parca[0];
                                        break;
                                    }
                                    else if (tcNo.Length != 11)
                                        Console.WriteLine("TC No 11 karakterli olmak zorunda!");
                                    else
                                        if (tamamiRakamMi(tcNo) == false)
                                        Console.WriteLine("TC No sadece rakamdan oluşmak zorunda");
                                    else
                                        break;
                                }




                                //AD

                                while (true)
                                {
                                    Console.WriteLine("Adı:" + parca[1] + "\n Yeni Şoför Adı Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Şofor Adı:");
                                    ad = Console.ReadLine().ToUpper();
                                    if (ad.Length == 0)
                                    {
                                        ad = parca[1];
                                        break;
                                    }
                                    else if (ad.Length < 2)
                                        Console.WriteLine("ad 2 karakterden az olamaz!");
                                    else
                                        if (tamamiKarakterMi(ad) == false)
                                    {
                                        Console.WriteLine("Adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //SOYAD

                                while (true)
                                {
                                    Console.WriteLine("Soyadı:" + parca[2] + "\n Yeni Şoför Soyadı Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Şofor Soyadı:");
                                    soyad = Console.ReadLine().ToUpper();
                                    if (soyad.Length == 0)
                                    {
                                        soyad = parca[2];
                                        break;
                                    }
                                    else if (soyad.Length < 2)
                                        Console.WriteLine("Soyad 2 karakterden az olamaz");
                                    else
                                        if (tamamiKarakterMi(soyad) == false)
                                    {
                                        Console.WriteLine("Soyadı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //GOREVİ

                                while (true)
                                {
                                    Console.WriteLine("Görevi:" + parca[3] + "\n Yeni Şoför Görevi Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Şofor Görevi:");
                                    gorevi = Console.ReadLine().ToUpper();
                                    if (gorevi.Length == 0)
                                    {
                                        gorevi = parca[3];
                                        break;
                                    }
                                    else if (gorevi.Length <= 10 || gorevi.Length >= 30)
                                        Console.WriteLine("Görevi en az 10 en fazla 30 karakter olmak zorunda!");
                                    else
                                        if (tamamiKarakterMi(gorevi) == false)
                                    {
                                        Console.WriteLine("Görevi  sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }
                                string bilgi = tcNo + "#" + ad + "#" + soyad + "#" + gorevi;
                                Sofor[sira] = bilgi;
                                StreamWriter sw = new StreamWriter("Sofor.ac", false, Encoding.Default);
                                foreach (var item in Sofor)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();

                                Console.WriteLine("Şoför bilgileri güncellendi, tekrar güncellemek istiyor musunuz?(E-H)");
                                tekrarSofor = Console.ReadLine().ToUpper();

                            } while (tekrarSofor == "E");



                        }
                        else if (secim == "4")
                        {
                            Console.WriteLine("---TÜMÜNÜ SİLME İŞLEMLERİ---");
                            Console.Write("Dosyadaki" + Sofor.Count + "Şoför Kaydı Silinecektir. \nDevam Etmek İstiyor musun?(E-H) ");
                            string cevap = Console.ReadLine().ToUpper();
                            if (cevap == "E")
                            {
                                Sofor.Clear();
                                StreamWriter sw = new StreamWriter("Sofor.ac", false, Encoding.Default);
                                sw.Close();
                                Console.WriteLine("Dosyadaki Şoförler Silindi.Lütfen Devam Etmek İçin Bir Tuşa Basınız...");
                            }
                            else
                            {
                                Console.WriteLine("İşlem İptal Edildi.");
                            }
                            Console.ReadKey();
                        }
                        else if (secim == "5")
                        {
                            string tekrarSofor;
                            do
                            {
                                Console.WriteLine("---ŞOFÖR ARAYA EKLEME İŞLEMLERİ---");
                                Console.WriteLine("Dosyada Kayıtlı Şoförler");
                                Console.WriteLine("TC No\t\t|Ad\t|Soyad\t\t|Görevi");
                                int sayac = 1;
                                foreach (var item in Sofor)
                                {
                                    string[] parcaa = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parcaa[0] + "\t|" + parcaa[1] + "\t|" + parcaa[2] + "\t|" + parcaa[3] + "\t");
                                }
                                int sira;
                                while (true)
                                {
                                    Console.WriteLine("Araya eklenecek sıra numarasını giriniz:");
                                    if (int.TryParse(Console.ReadLine(), out sira) == false)
                                        Console.WriteLine("Sıra Numarası Düzgün Girilmemiş!\nTekrar...");
                                    else
                                    {
                                        if (sira < 1 || sira > sayac - 1)
                                        {
                                            Console.WriteLine("Olmayan Sıra Numarası Girilmiş!\nTekrar...");
                                        }
                                        else
                                            break;
                                    }
                                }
                                string tcNo, ad, soyad, gorevi;
                                Console.WriteLine("EKLEME İŞLEMLERİ");


                                while (true)
                                {
                                    Console.WriteLine("TC No:");
                                    tcNo = Console.ReadLine();
                                    if (tcNo.Length != 11)
                                        Console.WriteLine("TC No 11 karakterli olmak zorunda!");
                                    else
                                        if (tamamiRakamMi(tcNo) == false)
                                        Console.WriteLine("TC No sadece rakamdan oluşmak zorunda");
                                    else
                                        break;
                                }




                                //AD

                                while (true)
                                {
                                    Console.WriteLine("Adı:");
                                    ad = Console.ReadLine().ToUpper();
                                    if (ad.Length <= 2 || ad.Length > 10)
                                        Console.WriteLine("ad 2 karakterden az olamaz!");
                                    else
                                         if (tamamiKarakterMi(ad) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //SOYAD

                                while (true)
                                {
                                    Console.WriteLine("Soyadı:");
                                    soyad = Console.ReadLine().ToUpper();
                                    if (soyad.Length <= 2 || soyad.Length > 15)
                                        Console.WriteLine("Soyad 2 karakterden az olamaz");
                                    else
                                         if (tamamiKarakterMi(soyad) == false)
                                    {
                                        Console.WriteLine("Şoför soyadı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //GÖREVİ

                                while (true)
                                {
                                    Console.WriteLine("Görevi:");
                                    gorevi = Console.ReadLine().ToUpper();
                                    if (gorevi.Length <= 9 || gorevi.Length >= 30)
                                        Console.WriteLine("Görevi en az 10 en fazla 30 karakter olmak zorunda!");
                                    else
                                         if (tamamiKarakterMi(gorevi) == false)
                                    {
                                        Console.WriteLine("Şoför görevi sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }
                                string bilgi = tcNo + "#" + ad + "#" + soyad + "#" + gorevi;
                                Sofor.Insert(sira - 1, bilgi);
                                StreamWriter sw = new StreamWriter("Sofor.ac", false, Encoding.Default);
                                foreach (var item in Sofor)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();

                                Console.WriteLine("Araya Şoför bilgileri eklendi, yeni eklemek istiyor musunuz?(E-H)");
                                tekrarSofor = Console.ReadLine().ToUpper();
                            } while (tekrarSofor == "E");

                        }
                        else if (secim == "6")
                        {
                            string tekrarSofor;
                            do
                            {
                                Console.WriteLine("---ARAMA İŞLEMLERİ---");
                                Console.WriteLine("Arama Yapılcak Özellik Seçiniz:");
                                Console.WriteLine("1) TC No");
                                Console.WriteLine("2) Şoför Ad");
                                Console.WriteLine("3) Tümünde Ara");
                                Console.WriteLine("Seçiminiz:");

                                secim = Console.ReadLine();
                                while (secim != "1" && secim != "2" && secim != "3")
                                {
                                    Console.WriteLine("Lütfen sadece 1, 2 veya 3 giriniz.");
                                    secim = Console.ReadLine();
                                }

                                Console.WriteLine("Aranacak değeri giriniz:");
                                string ara = Console.ReadLine().ToUpper();
                                int sayac = 0;

                                foreach (var item in Sofor)
                                {

                                    string[] aparca = item.Split('#');
                                    if (secim == "1" && aparca[0].Contains(ara))
                                    {

                                        sayac++;

                                        Console.WriteLine(sayac + ")-" + item);

                                    }
                                    else if (secim == "2" && aparca[1].Contains(ara))
                                    {
                                        sayac++;
                                        Console.WriteLine(sayac + ")-" + item);

                                    }

                                    else if (secim == "3" && item.Contains(ara))
                                    {
                                        sayac++;
                                        Console.WriteLine(sayac + ")-" + item);
                                    }

                                }

                                if (sayac == 0)
                                {
                                    Console.WriteLine("Kayıt bulunamadı.");
                                }
                                else
                                {
                                    Console.WriteLine(sayac + " kayıt bulundu...");
                                }
                                Console.WriteLine("Şoför bilgileri bulundu, tekrar bulmak istiyor musunuz?(E-H)");
                                tekrarSofor = Console.ReadLine().ToUpper();

                            } while (tekrarSofor == "E");
                        }
                        else if (secim == "7")
                        {
                            Console.WriteLine("---SIRALAMA İŞLEMLERİ---");
                            Console.WriteLine("1-Kayıtları Geçici Olarak Sıralı Listele");
                            Console.WriteLine("2-Kayıtları Kalıcı Olarak Sırala ve Listele");
                            Console.WriteLine("Seçiminiz:");
                            secim = Console.ReadLine();
                            if (secim == "1")
                            {
                                List<string> SoforKopya = new List<string>();
                                foreach (var item in Sofor)
                                {
                                    SoforKopya.Add(item);
                                }
                                SoforKopya.Sort();
                                foreach (var item in SoforKopya)
                                {

                                    Console.WriteLine(item);

                                }
                                Console.WriteLine("Kayıtlar Geçici Olarak Sıralandı...");

                            }
                            else if (secim == "2")
                            {
                                Sofor.Sort();
                                StreamWriter sw = new StreamWriter("Sofor.ac", false, Encoding.Default);
                                foreach (var item in Sofor)
                                {
                                    sw.WriteLine(item);
                                    Console.WriteLine(item);

                                }
                                sw.Close();
                                Console.WriteLine("Kayıtlar Kalıcı Olarak Sıralandı...");
                            }
                            Console.WriteLine("Devam etmek için bir tuşa basın...");
                            Console.ReadKey();
                        }
                        else if (secim == "8")
                        {
                            Console.WriteLine("---KAYITLI ŞOFÖR BİLGİLERİNİ LİSTELEME---");
                            Console.WriteLine("TC No\t\t|Ad\t|Soyad\t\t|Görevi");
                            int say = 1;
                            foreach (var item in Sofor)
                            {
                                string[] parca = item.Split('#');
                                Console.WriteLine(say++ + ")" + parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t");
                            }

                        }
                        else if (secim == "9")
                        {
                            Console.Clear();
                            break;
                        }
                        else if (secim == "0")
                        {
                            Environment.Exit(0);
                            break;
                        }
                        else
                            Console.WriteLine("Hatalı Seçim Ypatınız. Lütfen tekrar deneyiniz.\n Devam etmek için bir tuşa basınız... ");
                            Console.ReadKey();
                            Console.Clear();
                        break;
                    }
                }
                else if (secim == "A")
                {
                    if (File.Exists("Arac.ac") == false)
                    {
                        StreamWriter sw = new StreamWriter("Arac.ac", true, Encoding.Default);
                        sw.Close();
                    }
                    StreamReader sr = new StreamReader("Arac.ac", Encoding.Default);
                    string okunan;
                    Console.WriteLine("Kayıtlı Araç Bilgileri");
                    Console.WriteLine("Model\t|Araç Tür\t|Plaka No\t|Şoför");
                    while ((okunan = sr.ReadLine()) != null)
                    {
                        Arac.Add(okunan);
                        string[] parca = okunan.Split('#');
                        Console.WriteLine(parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t");
                    }
                    sr.Close();

                    while (true)
                    {

                        Console.WriteLine("~Araç Tablosu İşlemleri");
                        Console.WriteLine("1-Ekleme\r\n 2-Silme\r\n 3-Güncelleme\r\n 4-Tümünü Silme\r\n 5-Araya Ekle\r\n 6-Bul\r\n 7-Sırala\r\n 8-Listeleme\r\n 9-Ana Menüye Dön\r\n 0-Çıkış");
                        Console.WriteLine("Seçiminiz:");
                        secim = Console.ReadLine().ToUpper();

                        if (secim == "1")
                        {
                            string model, aracTur, plakaNo, aSofor, tekrarArac;
                            Console.WriteLine("---EKLEME İŞLEMLERİ---");
                            do
                            {
                                //MODEL
                                while (true)
                                {
                                    Console.WriteLine("Model:");
                                    model = Console.ReadLine().ToUpper();
                                    if (model.Length <= 2 || model.Length >= 11)
                                        Console.WriteLine("Model 3 den az 11 den büyük karakterli olamaz!");
                                    else
                                        break;
                                }




                                //ARAÇ TÜR

                                while (true)
                                {
                                    Console.WriteLine("Araç Türü:");
                                    aracTur = Console.ReadLine().ToUpper();
                                    if (aracTur.Length <= 4 || aracTur.Length >= 12)
                                        Console.WriteLine("araç türü 5 karakterden az 12 dan büyük olamaz!");
                                    else
                                         if (tamamiKarakterMi(aracTur) == false)
                                    {
                                        Console.WriteLine("Araç türü sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //PLAKA NO

                                while (true)
                                {
                                    Console.WriteLine("Plaka No:");
                                    plakaNo = Console.ReadLine().ToUpper();
                                    if (plakaNo.Length != 7 && plakaNo.Length != 8)
                                        Console.WriteLine("Plaka No 7 karakter ya da 8 karakterli olmalı");
                                    else
                                        break;


                                }




                                //ŞOFÖR

                                while (true)
                                {
                                    Console.WriteLine("Şoför:");
                                    aSofor = Console.ReadLine().ToUpper();
                                    if (aSofor.Length <= 2 || aSofor.Length >= 12)
                                        Console.WriteLine("Şoför  2 den fazla 12 karakter olmak zorunda!");
                                    else
                                        if (tamamiKarakterMi(aSofor) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }
                                string bilgi = model + "#" + aracTur + "#" + plakaNo + "#" + aSofor;
                                Arac.Add(bilgi);
                                StreamWriter sw = new StreamWriter("Arac.ac", true, Encoding.Default);
                                sw.WriteLine(bilgi);
                                sw.Close();

                                Console.WriteLine("Araç bilgileri eklendi, yeni eklemek istiyor musunuz?(E-H)");
                                tekrarArac = Console.ReadLine().ToUpper();
                            } while (tekrarArac == "E");
                        }
                        else if (secim == "2")
                        {
                            string tekrarArac;
                            do
                            {


                                Console.WriteLine("---SİLME İŞLEMLERİ---");
                                int sayac = 1;
                                foreach (var item in Arac)
                                {
                                    string[] parca = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t");
                                }
                                Console.WriteLine("Silmek istediğiniz satır numarasını giriniz:");
                                int sira = Convert.ToInt32(Console.ReadLine());
                                Arac.RemoveAt(sira - 1);


                                StreamWriter sw = new StreamWriter("Arac.ac", false, Encoding.Default);
                                foreach (var item in Arac)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();
                                Console.WriteLine("Aracınız Silindi,yine silmek istiyor musunuz?(E-H)");

                                tekrarArac = Console.ReadLine().ToUpper();
                            } while (tekrarArac == "E");
                        }
                        else if (secim == "3")
                        {
                            string tekrarArac;
                            do
                            {


                                Console.WriteLine("---ARAÇ GÜNCELLEME İŞLEMLERİ---");
                                Console.WriteLine("Dosyada Kayıtlı Araçlar");
                                Console.WriteLine("Model\t\t|Araç Tür\t|Plaka No\t\t|Şoför");
                                int sayac = 1;
                                foreach (var item in Arac)
                                {
                                    string[] parcaa = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parcaa[0] + "\t|" + parcaa[1] + "\t|" + parcaa[2] + "\t|" + parcaa[3] + "\t");
                                }
                                int sira;
                                while (true)
                                {
                                    Console.WriteLine("Güncellenecek istediğiniz satır numarasını giriniz:");
                                    if (int.TryParse(Console.ReadLine(), out sira) == false)
                                        Console.WriteLine("Sıra Numarası Düzgün Girilmemiş!\nTekrar giriniz...");
                                    else
                                    {
                                        if (sira < 1 || sira > sayac - 1)
                                        {
                                            Console.WriteLine("Olmayan Sıra Numarası Girilmiş!\nTekrar...");
                                        }
                                        else
                                            break;
                                    }
                                }
                                string model, aracTur, plakaNo, aSofor;
                                sira = sira - 1;
                                string[] parca = Arac[sira].Split('#');
                                //MODEL
                                while (true)
                                {
                                    Console.WriteLine("Model:" + parca[0] + "\n Yeni Modeli Tc Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Araç Modeli:");
                                    model = Console.ReadLine().ToUpper();
                                    if (model.Length == 0)
                                    {
                                        model = parca[0];
                                        break;
                                    }
                                    else if (model.Length <= 2 || model.Length >= 11)
                                        Console.WriteLine("Model 3 den az 11 den büyük karakterli olamaz!");
                                    else
                                        break;
                                }




                                //ARAÇ TÜR

                                while (true)
                                {
                                    Console.WriteLine("Araç Türü:" + parca[1] + "\n Yeni Araç Türü Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Araç Türü:");
                                    aracTur = Console.ReadLine().ToUpper();
                                    if (aracTur.Length == 0)
                                    {
                                        aracTur = parca[1];
                                        break;
                                    }
                                    else if (aracTur.Length <= 4 || aracTur.Length >= 12)
                                        Console.WriteLine("Araç türü 5 karakterden az 12 dan büyük olamaz!");
                                    else
                                         if (tamamiKarakterMi(aracTur) == false)
                                    {
                                        Console.WriteLine("Araç türü sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //PLAKA NO

                                while (true)
                                {
                                    Console.WriteLine("Plaka No:" + parca[2] + "\n Yeni Plaka No Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Plaka No:");
                                    plakaNo = Console.ReadLine().ToUpper();
                                    if (plakaNo.Length == 0)
                                    {
                                        plakaNo = parca[2];
                                        break;
                                    }
                                    else if (plakaNo.Length != 7 && plakaNo.Length != 8)
                                        Console.WriteLine("Plaka No 7 karakter ya da 8 karakterli olmalı");
                                    else
                                        break;
                                }




                                //ŞOFÖR

                                while (true)
                                {
                                    Console.WriteLine("Şoför:" + parca[3] + "\n Yeni Şoför  Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Şofor:");
                                    aSofor = Console.ReadLine().ToUpper();
                                    if (aSofor.Length == 0)
                                    {
                                        aSofor = parca[3];
                                        break;
                                    }
                                    else if (aSofor.Length <= 2 || aSofor.Length >= 12)
                                        Console.WriteLine("Şoför  2 den fazla 12 karakter olmak zorunda!");
                                    else
                                         if (tamamiKarakterMi(aSofor) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }
                                string bilgi = model + "#" + aracTur + "#" + plakaNo + "#" + aSofor;
                                Arac[sira] = bilgi;
                                StreamWriter sw = new StreamWriter("Arac.ac", false, Encoding.Default);
                                foreach (var item in Arac)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();
                                Console.WriteLine("Aracınız Güncellendi,Lütfen devam etmek için bir tuşa basınız...");
                                Console.ReadKey();
                                Console.WriteLine("Araç bilgileri güncellendi, tekrar güncellemek istiyor musunuz?(E-H)");
                                tekrarArac = Console.ReadLine().ToUpper();

                            } while (tekrarArac == "E");
                        }
                        else if (secim == "4")
                        {
                            Console.WriteLine("---TÜMÜNÜ SİLME İŞLEMLERİ---");
                            Console.Write("Dosyadaki" + Arac.Count + "Araç Kaydı Silinecektir. \nDevam Etmek İstiyor musun?(E-H) ");
                            string cevap = Console.ReadLine().ToUpper();
                            if (cevap == "E")
                            {
                                Arac.Clear();
                                StreamWriter sw = new StreamWriter("Arac.ac", false, Encoding.Default);
                                sw.Close();
                                Console.WriteLine("Dosyadaki Araçlar Silindi.Lütfen Devam Etmek İçin Bir Tuşa Basınız...");
                            }
                            else
                            {
                                Console.WriteLine("İşlem İptal Edildi.");
                            }
                            Console.ReadKey();
                        }
                        else if (secim == "5")
                        {
                            string tekrarArac;
                            do
                            {


                                Console.WriteLine("---ARAÇ ARAYA EKLEME İŞLEMLERİ---");
                                Console.WriteLine("Dosyada Kayıtlı Araç");
                                Console.WriteLine("Model\t|Araç Türü\t|Plaka No\t|Şoför");
                                int sayac = 1;
                                foreach (var item in Arac)
                                {
                                    string[] parcaa = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parcaa[0] + "\t|" + parcaa[1] + "\t|" + parcaa[2] + "\t|" + parcaa[3] + "\t");
                                }
                                int sira;
                                while (true)
                                {
                                    Console.WriteLine("Araya eklenecek sıra numarasını giriniz:");
                                    if (int.TryParse(Console.ReadLine(), out sira) == false)
                                        Console.WriteLine("Sıra Numarası Düzgün Girilmemiş!\nTekrar...");
                                    else
                                    {
                                        if (sira < 1 || sira > sayac - 1)
                                        {
                                            Console.WriteLine("Olmayan Sıra Numarası Girilmiş!\nTekrar...");
                                        }
                                        else
                                            break;
                                    }
                                }
                                string model, aracTur, plakaNo, aSofor;
                                Console.WriteLine("EKLEME İŞLEMLERİ");

                                //MODEL
                                while (true)
                                {
                                    Console.WriteLine("Model:");
                                    model = Console.ReadLine().ToUpper();
                                    if (model.Length <= 2 || model.Length >= 11)
                                        Console.WriteLine("Model 3 den az 11 den büyük karakterli olamaz!");
                                    else
                                        break;
                                }




                                //ADRAÇ TÜR

                                while (true)
                                {
                                    Console.WriteLine("Araç Türü:");
                                    aracTur = Console.ReadLine().ToUpper();
                                    if (aracTur.Length <= 4 || aracTur.Length >= 12)
                                        Console.WriteLine("araç türü 5 karakterden az 12 dan büyük olamaz!");
                                    else
                                         if (tamamiKarakterMi(aracTur) == false)
                                    {
                                        Console.WriteLine("Araç türü sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }




                                //PLAKA NO

                                while (true)
                                {
                                    Console.WriteLine("Plaka No:");
                                    plakaNo = Console.ReadLine().ToUpper();
                                    if (plakaNo.Length != 7 && plakaNo.Length != 8)
                                        Console.WriteLine("Plaka No 7 karakter ya da 8 karakterli olmalı");
                                    else
                                        break;
                                }




                                //ŞOFÖR

                                while (true)
                                {
                                    Console.WriteLine("Şoför:");
                                    aSofor = Console.ReadLine().ToUpper();
                                    if (aSofor.Length <= 2 || aSofor.Length >= 12)
                                        Console.WriteLine("Şoför  2 den fazla 12 karakter olmak zorunda!");
                                    else
                                        if (tamamiKarakterMi(aSofor) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }
                                string bilgi = model + "#" + aracTur + "#" + plakaNo + "#" + aSofor;
                                Arac.Insert(sira - 1, bilgi);
                                StreamWriter sw = new StreamWriter("Arac.ac", false, Encoding.Default);
                                foreach (var item in Arac)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();
                                Console.WriteLine("Araya Araç Eklendi.Tekrar eklemek istiyor musunuz?(E-H)");
                                tekrarArac = Console.ReadLine().ToUpper();
                            } while (tekrarArac == "E");
                        }
                        else if (secim == "6")
                        {
                            Console.WriteLine("---ARAMA İŞLEMLERİ---");
                            string tekrarArac;
                            do
                            {
                                Console.WriteLine("Arama Yapılcak Özellik Seçiniz:");
                                Console.WriteLine("1) Plaka No");
                                Console.WriteLine("2) Şoför Ad");
                                Console.WriteLine("3) Tümünde Ara");
                                Console.WriteLine("Seçiminiz:");
                                secim = Console.ReadLine();
                                while (secim != "1" && secim != "2" && secim != "3")
                                {
                                    Console.WriteLine("Lütfen sadece 1, 2 veya 3 giriniz.");
                                    secim = Console.ReadLine().ToUpper();
                                }
                                Console.Write("Aranacak değer:");
                                int sayac = 0;
                                string ara = Console.ReadLine().ToUpper();
                                foreach (var item in Arac)
                                {
                                    string[] aparca = item.Split('#');
                                    if (secim == "1")
                                    {
                                        if (aparca[2].Contains(ara) == true)
                                        {
                                            sayac++;
                                            Console.WriteLine(sayac + ")-" + item);
                                        }

                                    }
                                    else if (secim == "2")
                                    {
                                        if (aparca[3].Contains(ara.ToUpper()) == true)
                                        {
                                            sayac++;
                                            Console.WriteLine(sayac + ")-" + item);

                                        }

                                    }
                                    else if (secim == "3")
                                    {
                                        if (item.Contains(ara) == true)
                                        {
                                            sayac++;
                                            Console.WriteLine(sayac + ")-" + item);
                                        }
                                    }


                                }
                                if (sayac == 0)
                                    Console.WriteLine("kayıt bulunamadı");
                                else
                                    Console.WriteLine(sayac + "kayıt bulundu...");
                                Console.WriteLine("Araç bilgileri bulundu, tekrar bulmak istiyor musunuz?(E-H)");
                                tekrarArac = Console.ReadLine().ToUpper();

                            } while (tekrarArac == "E");


                        }
                        else if (secim == "7")
                        {
                            Console.WriteLine("---SIRALAMA İŞLEMLERİ---");
                            Console.WriteLine("1-Kayıtları Geçici Olarak Sıralı Listele");
                            Console.WriteLine("2-Kayıtları Kalıcı Olarak Sırala ve Listele");
                            Console.WriteLine("Seçiminiz:");
                            secim = Console.ReadLine();
                            if (secim == "1")
                            {
                                List<string> AracKopya = new List<string>();
                                foreach (var item in Arac)
                                {
                                    AracKopya.Add(item);
                                }
                                AracKopya.Sort();
                                foreach (var item in AracKopya)
                                {

                                    Console.WriteLine(item);

                                }
                                Console.WriteLine("Kayıtlar Geçici Olarak Sıralandı...");

                            }
                            else if (secim == "2")
                            {
                                Arac.Sort();
                                StreamWriter sw = new StreamWriter("Arac.ac", false, Encoding.Default);
                                foreach (var item in Arac)
                                {
                                    sw.WriteLine(item);
                                    Console.WriteLine(item);

                                }
                                sw.Close();
                                Console.WriteLine("Kayıtlar Kalıcı Olarak Sıralandı...");
                            }
                            Console.WriteLine("Devam etmek için bir tuşa basın...");
                            Console.ReadKey();
                        }
                        else if (secim == "8")
                        {
                            Console.WriteLine("---KAYITLI ARAÇ BİLGİLERİNİ LİSTELEME---");
                            Console.WriteLine("Model\t\t|Araç Türü\t|Plaka No\t|Şoför");
                            int say = 1;
                            foreach (var item in Arac)
                            {
                                string[] parca = item.Split('#');
                                Console.WriteLine(say++ + ")" + parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t");
                            }
                        }
                        else if (secim == "9")
                        {
                            Console.Clear();
                            break;
                        }
                        else if (secim == "0")
                        {
                            Environment.Exit(0);
                            break;
                        }
                        else
                            Console.WriteLine("Hatalı Seçim Ypatınız. Lütfen tekrar deneyiniz.\n Devam etmek için bir tuşa basınız... ");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    }
                }
                else if (secim == "G")
                {
                    if (File.Exists("Gorev.ac") == false)
                    {
                        StreamWriter sw = new StreamWriter("Gorev.ac", true, Encoding.Default);
                        sw.Close();
                    }
                    StreamReader sr = new StreamReader("Gorev.ac", Encoding.Default);
                    string okunan;
                    Console.WriteLine("Kayıtlı Görev Bilgileri");
                    Console.WriteLine("Gidilen Yer\t|Saat\t|Araç Plaka\t|Şoför\t\t|Tarihi");
                    while ((okunan = sr.ReadLine()) != null)
                    {
                        Gorev.Add(okunan);
                        string[] parca = okunan.Split('#');
                        Console.WriteLine(parca[0] + "|\t\t" + parca[1] + "|\t|" + parca[2] + "\t|" + parca[3] + "\t\t" + parca[4] + "\t");
                    }
                    sr.Close();

                    while (true)
                    {
                        Console.WriteLine("~Görev Tablosu İşlemleri");
                        Console.WriteLine("1-Ekleme\r\n 2-Silme\r\n 3-Güncelleme\r\n 4-Tümünü Silme\r\n 5-Araya Ekle\r\n 6-Bul\r\n 7-Sırala\r\n 8-Listeleme\r\n 9-Ana Menüye Dön\r\n 0-Çıkış");
                        Console.WriteLine("Seçiminiz:");
                        secim = Console.ReadLine().ToUpper();

                        if (secim == "1")
                        {
                            string gidilenYer, saat, aracPlaka, gSofor, tekrarGorev;
                            DateTime tarihi;

                            Console.WriteLine("---EKLEME İŞLEMLERİ---");
                            do
                            {
                                //GİDİLEN YER
                                while (true)
                                {
                                    Console.WriteLine("Gidilen Yer:");
                                    gidilenYer = Console.ReadLine().ToUpper();
                                    if (gidilenYer.Length <= 3 || gidilenYer.Length >= 11)
                                        Console.WriteLine("Gidilen yer 4 den az 11 den büyük karakterli olamaz!");
                                    else
                                        if (tamamiKarakterMi(gidilenYer) == false)
                                    {
                                        Console.WriteLine("Gidilen yer sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }








                                //SAAT 

                                while (true)
                                {
                                    Console.WriteLine("Saat (ss:dd):");
                                    saat = Console.ReadLine();
                                    int s, d;
                                    if (saat.Length != 5 || saat[2] != ':' || int.TryParse(saat.Substring(0, 2), out s) == false || int.TryParse(saat.Substring(3, 2), out d) == false || s < 0 || s > 24 || d < 0 || d > 59)//2. karakter : değilse
                                        Console.WriteLine("Girdiğiniz bilgiler saat formatına uygun değil!");
                                    else
                                        break;
                                }




                                //ARACPLAKA

                                while (true)
                                {
                                    Console.WriteLine("Araç Plaka:");
                                    aracPlaka = Console.ReadLine().ToUpper();
                                    if (aracPlaka.Length != 7 && aracPlaka.Length != 8)
                                        Console.WriteLine("Araç Plakası  7 karakter ya da 8 karakterli olmalı");
                                    else
                                        break;
                                }







                                //ŞOFÖR

                                while (true)
                                {
                                    Console.WriteLine("Şoför:");
                                    gSofor = Console.ReadLine().ToUpper();

                                    if (gSofor.Length < 2 || gSofor.Length > 13)
                                        Console.WriteLine("Şoför adı 2'den fazla ve 12'den az karakter olmak zorunda!");

                                    else if (tamamiKarakterMi(gSofor) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }



                                //TARİHİ 

                                while (true)
                                {
                                    Console.Write("Tarihi:");

                                    if (DateTime.TryParse(Console.ReadLine(), out tarihi) == false)
                                        Console.WriteLine("Girdiğiniz bilgiler tarih formatına uygun değil!!!!");
                                    else
                                    {
                                        if (tarihi > DateTime.Now)
                                            Console.WriteLine("Girilen tarih gelecek tarihli olamaz.!");
                                        else
                                            break;
                                    }
                                }


                                string bilgi = gidilenYer + "#" + saat + "#" + aracPlaka + "#" + gSofor + "#" + tarihi;
                                Gorev.Add(bilgi);
                                StreamWriter sw = new StreamWriter("Gorev.ac", true, Encoding.Default);
                                sw.WriteLine(bilgi);
                                sw.Close();

                                Console.WriteLine("Görev bilgileri eklendi, yeni eklemek istiyor musunuz?(E-H)");
                                tekrarGorev = Console.ReadLine().ToUpper();
                            } while (tekrarGorev == "E");

                        }
                        else if (secim == "2")
                        {
                            Console.WriteLine("---SİLME İŞLEMLERİ---");
                            int sayac = 1;
                            foreach (var item in Gorev)
                            {
                                string[] parca = item.Split('#');
                                Console.WriteLine(sayac++ + ")" + parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t" + parca[4] + "\t");
                            }
                            Console.WriteLine("Silmek istediğiniz satır numarasını giriniz:");
                            int sira = Convert.ToInt32(Console.ReadLine());
                            Gorev.RemoveAt(sira - 1);


                            StreamWriter sw = new StreamWriter("Gorev.ac", false, Encoding.Default);
                            foreach (var item in Arac)
                            {
                                sw.WriteLine(item);

                            }
                            sw.Close();
                            Console.WriteLine("Göreviniz Silindi,Lütfen devam etmek için bir tuşa basınız...");
                            Console.ReadKey();

                        }
                        else if (secim == "3")
                        {
                            string tekrarGorev;
                            do
                            {
                                Console.WriteLine("---GÖREV GÜNCELLEME İŞLEMLERİ---");
                                Console.WriteLine("Dosyada Kayıtlı Görevler");
                                Console.WriteLine("Gidilen Yer\t|Saat\t|Araç Plaka\t\t|Şoför\t|Tarihi");
                                int sayac = 1;
                                foreach (var item in Gorev)
                                {
                                    string[] parcaa = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parcaa[0] + "\t|" + parcaa[1] + "\t|" + parcaa[2] + "\t|" + parcaa[3] + "\t|" + parcaa[4]);
                                }
                                int sira;
                                while (true)
                                {
                                    Console.WriteLine("Güncellenecek istediğiniz satır numarasını giriniz:");
                                    if (int.TryParse(Console.ReadLine(), out sira) == false)
                                        Console.WriteLine("Sıra Numarası Düzgün Girilmemiş!\nTekrar giriniz...");
                                    else
                                    {
                                        if (sira < 1 || sira > sayac - 1)
                                        {
                                            Console.WriteLine("Olmayan Sıra Numarası Girilmiş!\nTekrar...");
                                        }
                                        else
                                            break;
                                    }
                                }
                                string gidilenYer, saat, aracPlaka, gSofor;
                                DateTime tarihi;
                                sira = sira - 1;
                                string[] parca = Gorev[sira].Split('#');



                                //GİDİLEN YER
                                while (true)
                                {
                                    Console.WriteLine("Gidilen Yer:" + parca[0] + "\n Yeni Gidilcek Yeri Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Gidilcek Yer:");
                                    gidilenYer = Console.ReadLine().ToUpper();
                                    if (gidilenYer.Length == 0)
                                    {
                                        gidilenYer = parca[0];
                                        break;
                                    }
                                    else if (gidilenYer.Length <= 3 || gidilenYer.Length >= 11)
                                        Console.WriteLine("Gidilen yer 3 den az 11 den büyük karakterli olamaz!");
                                    else
                                        if (tamamiKarakterMi(gidilenYer) == false)
                                    {
                                        Console.WriteLine("Gidilen yer sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }





                                //SAAT

                                while (true)
                                {


                                    Console.Write("Saat:" + parca[1] + "\nYeni Saati Girin Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Saat:");
                                    saat = Console.ReadLine();
                                    if (saat == "")
                                    {
                                        saat = parca[1];
                                    }
                                    else
                                    {
                                        int s, d;
                                        if (saat.Length != 5 || saat[2] != ':' || int.TryParse(saat.Substring(0, 2), out s) == false || int.TryParse(saat.Substring(3, 2), out d) == false || s < 0 || s > 24 || d < 0 || d > 59)
                                            Console.WriteLine("Girilen değerler saat formatı için uygun değil.");

                                        else
                                            break;
                                    }

                                }


                                //ARAÇ PLAKA
                                while (true)
                                {
                                    Console.WriteLine("Araç Plaka:" + parca[2] + "\n Yeni Plaka  Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Plaka:");
                                    aracPlaka = Console.ReadLine().ToUpper();
                                    if (aracPlaka.Length == 0)
                                    {
                                        aracPlaka = parca[2];
                                        break;
                                    }
                                    else if (aracPlaka.Length != 7 && aracPlaka.Length != 8)
                                        Console.WriteLine("Plaka 7 karakter ya da 8 karakterli olmalı");
                                    else
                                        break;
                                }



                                //ŞOFÖR

                                while (true)
                                {
                                    Console.WriteLine("Şoför:" + parca[3] + "\n Yeni Şoför  Giriniz Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Şofor:");
                                    gSofor = Console.ReadLine().ToUpper();
                                    if (gSofor.Length == 0)
                                    {
                                        gSofor = parca[3];
                                        break;
                                    }
                                    else if (gSofor.Length <= 2 || gSofor.Length >= 15)
                                        Console.WriteLine("Şoför  2 den fazla 15 karakter olmak zorunda!");
                                    else
                                        if (tamamiKarakterMi(gSofor) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;

                                }




                                //TARİHİ 

                                while (true)
                                {
                                    Console.Write("Tarihi:" + parca[4] + "\nYeni Tarihi Girin Ya Da Güncellemeden Geçmek İçin Enter a Basınız\nYeni Tarih:");

                                    if (DateTime.TryParse(Console.ReadLine(), out tarihi) == false)
                                        Console.WriteLine("Girdiğiniz bilgiler tarih formatına uygun değil!!!!");
                                    else
                                    {
                                        if (tarihi > DateTime.Now)
                                            Console.WriteLine("Girilen tarih gelecek tarihli olamaz.!");
                                        else
                                            break;
                                    }
                                }

                                string bilgi = gidilenYer + "#" + saat + "#" + aracPlaka + "#" + gSofor + "#" + tarihi;
                                Gorev[sira] = bilgi;
                                StreamWriter sw = new StreamWriter("Gorev.ac", false, Encoding.Default);
                                foreach (var item in Gorev)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();

                                Console.WriteLine("Görev bilgileri güncellendi, tekrar güncellemek istiyor musunuz?(E-H)");
                                tekrarGorev = Console.ReadLine().ToUpper();

                            } while (tekrarGorev == "E");
                        }
                        else if (secim == "4")
                        {
                            Console.WriteLine("---TÜMÜNÜ SİLME İŞLEMLERİ---");
                            Console.Write("Dosyadaki" + Gorev.Count + "Görev Kaydı Silinecektir. \nDevam Etmek İstiyor musun?(E-H) ");
                            string cevap = Console.ReadLine().ToUpper();
                            if (cevap == "E")
                            {
                                Gorev.Clear();
                                StreamWriter sw = new StreamWriter("Gorev.ac", false, Encoding.Default);
                                sw.Close();
                                Console.WriteLine("Dosyadaki Araçlar Silindi.Lütfen Devam Etmek İçin Bir Tuşa Basınız...");
                            }
                            else
                            {
                                Console.WriteLine("İşlem İptal Edildi.");
                            }
                            Console.ReadKey();
                        }
                        else if (secim == "5")
                        {
                            string tekrarGorev;
                            do
                            {


                                Console.WriteLine("---GÖREV ARAYA EKLEME İŞLEMLERİ---");
                                Console.WriteLine("Dosyada Kayıtlı Görevler");
                                Console.WriteLine("Gidilen Yer\t|Saat\t|Araç Plaka\t|Şoför\t|Tarih");
                                int sayac = 1;
                                foreach (var item in Gorev)
                                {
                                    string[] parcaa = item.Split('#');
                                    Console.WriteLine(sayac++ + ")" + parcaa[0] + "\t|" + parcaa[1] + "\t|" + parcaa[2] + "\t|" + parcaa[3] + "\t" + parcaa[4]);
                                }
                                int sira;
                                while (true)
                                {
                                    Console.WriteLine("Araya eklenecek sıra numarasını giriniz:");
                                    if (int.TryParse(Console.ReadLine(), out sira) == false)
                                        Console.WriteLine("Sıra Numarası Düzgün Girilmemiş!\nTekrar...");
                                    else
                                    {
                                        if (sira < 1 || sira > sayac - 1)
                                        {
                                            Console.WriteLine("Olmayan Sıra Numarası Girilmiş!\nTekrar...");
                                        }
                                        else
                                            break;
                                    }
                                }
                                string gidilenYer, saat, aracPlaka, gSofor;
                                DateTime tarihi;

                                //GİDİLEN YER
                                while (true)
                                {
                                    Console.WriteLine("Gidilen Yer:");
                                    gidilenYer = Console.ReadLine().ToUpper();
                                    if (gidilenYer.Length <= 3 || gidilenYer.Length >= 11)
                                        Console.WriteLine("Gidilen yer 4 den az 11 den büyük karakterli olamaz!");
                                    else
                                        if (tamamiKarakterMi(gidilenYer) == false)
                                    {
                                        Console.WriteLine("Gidilen yer sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }








                                //SAAT 

                                while (true)
                                {
                                    Console.WriteLine("Saat (ss:dd):");
                                    saat = Console.ReadLine();
                                    int s, d;
                                    if (saat.Length != 5 || saat[2] != ':' || int.TryParse(saat.Substring(0, 2), out s) == false || int.TryParse(saat.Substring(3, 2), out d) == false || s < 0 || s > 24 || d < 0 || d > 59)//2. karakter : değilse
                                        Console.WriteLine("Girdiğiniz bilgiler saat formatına uygun değil!");
                                    else
                                        break;
                                }




                                //ARACPLAKA

                                while (true)
                                {
                                    Console.WriteLine("Araç Plaka:");
                                    aracPlaka = Console.ReadLine().ToUpper();
                                    if (aracPlaka.Length != 7 && aracPlaka.Length != 8)
                                        Console.WriteLine("Araç Plakası  7 karakter ya da 8 karakterli olmalı");
                                    else
                                        break;
                                }







                                //ŞOFÖR

                                while (true)
                                {
                                    Console.WriteLine("Şoför:");
                                    gSofor = Console.ReadLine().ToUpper();

                                    if (gSofor.Length < 2 || gSofor.Length > 13)
                                        Console.WriteLine("Şoför adı 2'den fazla ve 12'den az karakter olmak zorunda!");

                                    else if (tamamiKarakterMi(gSofor) == false)
                                    {
                                        Console.WriteLine("Şoför adı sadece metinsel olabilir.");
                                    }
                                    else
                                        break;
                                }



                                //TARİHİ  

                                while (true)
                                {
                                    Console.Write("Tarihi:");

                                    if (DateTime.TryParse(Console.ReadLine(), out tarihi) == false)
                                        Console.WriteLine("Girdiğiniz bilgiler tarih formatına uygun değil!!!!");
                                    else
                                    {
                                        if (tarihi > DateTime.Now)
                                            Console.WriteLine("Girilen tarih gelecek tarihli olamaz.!");
                                        else
                                            break;
                                    }
                                }


                                string bilgi = gidilenYer + "#" + saat + "#" + aracPlaka + "#" + gSofor + "#" + tarihi;
                                Gorev.Insert(sira - 1, bilgi);
                                StreamWriter sw = new StreamWriter("Gorev.ac", false, Encoding.Default);
                                foreach (var item in Gorev)
                                {
                                    sw.WriteLine(item);

                                }
                                sw.Close();
                                Console.WriteLine("Araya Görev Eklendi.Tekrar eklemek istiyor musunuz?(E-H)");
                                tekrarGorev = Console.ReadLine().ToUpper();

                            } while (tekrarGorev == "E");
                        }
                        else if (secim == "6")
                        {
                            string tekrarGorev;
                            do
                            {


                                Console.WriteLine("---ARAMA İŞLEMLERİ---");
                                Console.WriteLine("Arama Yapılcak Özellik Seçiniz:");
                                Console.WriteLine("1) Araç Plaka");
                                Console.WriteLine("2) Şoför Ad");
                                Console.WriteLine("3) Tümünde Ara");
                                Console.WriteLine("Seçiminiz:");

                                secim = Console.ReadLine();
                                while (secim != "1" && secim != "2" && secim != "3")
                                {
                                    Console.WriteLine("Lütfen sadece 1, 2 veya 3 giriniz.");
                                    secim = Console.ReadLine();
                                }
                                Console.Write("Aranacak değer:");
                                int sayac = 0;
                                string ara = Console.ReadLine().ToUpper();
                                foreach (var item in Gorev)
                                {
                                    string[] aparca = item.Split('#');
                                    if (secim == "1")
                                    {
                                        if (aparca[2].Contains(ara) == true)
                                        {
                                            sayac++;
                                            Console.WriteLine(sayac + ")-" + item);
                                        }

                                    }
                                    else if (secim == "2")
                                    {
                                        if (aparca[3].Contains(ara) == true)
                                        {
                                            sayac++;
                                            Console.WriteLine(sayac + ")-" + item);

                                        }

                                    }
                                    else if (secim == "3")
                                    {
                                        if (item.Contains(ara) == true)
                                        {
                                            sayac++;
                                            Console.WriteLine(sayac + ")-" + item);
                                        }
                                    }


                                }
                                if (sayac == 0)
                                    Console.WriteLine("kayıt bulunamadı");
                                else
                                    Console.WriteLine(sayac + "kayıt bulundu...");
                                Console.WriteLine("Görev bilgileri bulundu, tekrar bulmak istiyor musunuz?(E-H)");
                                tekrarGorev = Console.ReadLine().ToUpper();

                            } while (tekrarGorev == "E");
                        }
                        else if (secim == "7")
                        {
                            Console.WriteLine("---SIRALAMA İŞLEMLERİ---");
                            Console.WriteLine("1-Kayıtları Geçici Olarak Sıralı Listele");
                            Console.WriteLine("2-Kayıtları Kalıcı Olarak Sırala ve Listele");
                            Console.WriteLine("Seçiminiz:");
                            secim = Console.ReadLine();
                            if (secim == "1")
                            {
                                List<string> GorevKopya = new List<string>();
                                foreach (var item in Arac)
                                {
                                    GorevKopya.Add(item);
                                }
                                GorevKopya.Sort();
                                foreach (var item in GorevKopya)
                                {

                                    Console.WriteLine(item);

                                }
                                Console.WriteLine("Kayıtlar Geçici Olarak Sıralandı...");

                            }
                            else if (secim == "2")
                            {
                                Gorev.Sort();
                                StreamWriter sw = new StreamWriter("Gorev.ac", false, Encoding.Default);
                                foreach (var item in Gorev)
                                {
                                    sw.WriteLine(item);
                                    Console.WriteLine(item);

                                }
                                sw.Close();
                                Console.WriteLine("Kayıtlar Kalıcı Olarak Sıralandı...");
                            }
                            Console.WriteLine("Devam etmek için bir tuşa basın...");
                            Console.ReadKey();

                        }
                        else if (secim == "8")
                        {
                            Console.WriteLine("---KAYITLI GÖREV BİLGİLERİNİ LİSTELEME---");
                            Console.WriteLine("Gidilcek Yer\t|Saat\t\t|Araç Plaka\t|Şoför\t|Tarih");
                            int say = 1;
                            foreach (var item in Gorev)
                            {
                                string[] parca = item.Split('#');
                                Console.WriteLine(say++ + ")" + parca[0] + "\t|" + parca[1] + "\t|" + parca[2] + "\t|" + parca[3] + "\t" + parca[4]);
                            }
                        }
                        else if (secim == "9")
                        {
                            Console.Clear();
                            break;
                        }
                        else if (secim == "0")
                        {
                            Environment.Exit(0);
                            break;
                        }

                        else
                            Console.WriteLine("Hatalı seçim yaptınız. Lütfen tekrar deneyiniz.\n Devam etmek için bir tuşa basınız...");
                            Console.ReadKey();
                        Console.Clear();
                    }

                }
                else if (secim == "Ç")
                {
                    Console.WriteLine("Çıkış Yapılıyor... Kapatmak için lütfen bir tuşa basın.");
                    break;
                }
                else
                {
                    Console.WriteLine("Hatalı Seçim Yaptınız");
                }



            }


            Console.ReadKey();
        }

        static bool tamamiKarakterMi(string metin)
        {
            foreach (var item in metin)
            {
                if (!(char.IsLetter(item) == true || item == ' '))
                    return false;
            }


            return true;
        }
        static bool tamamiRakamMi(string metin)
        {
            foreach (var item in metin)
            {
                if (char.IsDigit(item) == false)
                    return false;
            }


            return true;
        }

        public static string MD5Sifrele(string sifrelenecekMetin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);
            dizi = md5.ComputeHash(dizi);
            StringBuilder sb = new StringBuilder();

            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }



    }
}
