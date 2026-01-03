using UnityEngine;
using Terresquall;

public class DenizAltiKontrol : MonoBehaviour
{
    // ... (Diğer değişkenler ve Start() metodu aynı kalır)
    public float hareketHizi = 5f;
    public float donusHizi = 120f;

    [Header("Torpidolar")]
    [Tooltip("Ateşleme komutu için kullanılacak sanal tuş adı.")]
    public string ateslemeTusu = "Jump"; // Unity'de varsayılan Boşluk (Space) tuşudur.

    // Sahnede zaten var olan Torpido objesine referans.
    // Bu objeyi fırlatmadan önce Instantiate etmeyeceğiz, sadece 'Detach' edeceğiz.
    private GameObject torpidoObjesi;

    // Yeni bir atış için beklenen süre.
    private bool atisHazir = true;
    

    private Rigidbody rb;
    private float ileriGeriGiris;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Torpido objesini (adının "torpido" olduğundan emin olun) bul.
        // Bu objenin DenizAlti objesinin çocuğu olması gerekir.
        torpidoObjesi = transform.Find("torpido")?.gameObject;

        if (torpidoObjesi != null)
        {
            // Başlangıçta torpidonun görselini kapat ki görünmez olsun.
            torpidoObjesi.SetActive(false);

        }


    }
    void Update()
    {
        // 1. Joystick Girişlerini Alma:
        float joystickHorizontalGiris = VirtualJoystick.GetAxis("Horizontal"); // Joystick X ekseni (Dümen)
        float joystickVerticalGiris = VirtualJoystick.GetAxis("Vertical");     // Joystick Y ekseni (Hız/Güç)

        // 2. Kontrol Mantığını Uygulama:
        float donusGiris = joystickHorizontalGiris; // Horizontal girişi dönüş kontrolü olacak.
        ileriGeriGiris = joystickVerticalGiris;     // Vertical girişi ilerleme kontrolü olacak.

        // 3. Rotasyon Mantığı (Dümen Hareketi - X Ekseni Etrafında Dönüş):
        if (donusGiris != 0)
        {
            float rotasyonMiktari = donusGiris * donusHizi * Time.deltaTime;

            // X ekseninde dönüş (Pitch veya Roll değil, bu modelde Yaw görevi görüyor).
            // Dümen sola (negatif) basıldığında sola dönmesi için:
            // Sola basıldığında donusGiris negatif olur.
            // Sol: X ekseni etrafında pozitif dönüş (dönüş miktarını ters çeviriyoruz).

            // Transform.Rotate(X, Y, Z) kullanıyoruz. X ekseninde döndür.
            // Yönü düzeltmek için rotasyon miktarını ters çevirmemiz gerekebilir (eksi işareti ekleyerek).
            transform.Rotate(rotasyonMiktari, 0, 0, Space.Self);

            // Not: Space.Self, objenin kendi X ekseni etrafında dönmesini sağlar.
        }
        // ... (Rotasyon ve hareket giriş kodları burada devam eder)

        // Ateşleme Tuşu Kontrolü:
        // Hem tuşa basılmışsa hem de atış hazırsa ateşle.
        if (Input.GetMouseButtonDown(0) && atisHazir)
        {
            Atesle();
            print("ateşle çağırıldı");

        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 1. İlerleme Hareketi (Local Space):
        // İleri gitme yönü: Objenin Y ekseni (transform.up).
        if (ileriGeriGiris != 0)
        {
            Vector3 ileriHareketYonu = transform.up * ileriGeriGiris;
            rb.linearVelocity = ileriHareketYonu * hareketHizi;
        }
    }

    public void AtesEtButon()
    {
        // Butona basıldığında, atış hazırsa Ateşle() metodunu çağır.
        if (atisHazir)
        {
            Atesle();
        }
    }
    // Ateşleme Mantığı:
    void Atesle()
    {
        // ... (Bu kısım aynı kalır: torpidoyu aktif etme, parent'tan ayırma, Firlat çağırma, atisHazir = false yapma)
        if (torpidoObjesi == null) return;
        // if (!atisHazir) return; // Zaten AtesEtButon içinde kontrol ettik

        // 1. Torpidoyu Aktif Et ve Fırlat:
        torpidoObjesi.SetActive(true);

        // 2. Torpidoyu Fırlatma (Parent'tan ayırma):
        torpidoObjesi.transform.parent = null;

        // 3. Torpido Hareket script'ine fırlatma emrini veriyoruz.
        TorpidoHareketi hareketScripti = torpidoObjesi.GetComponent<TorpidoHareketi>();
        if (hareketScripti != null)
        {
            hareketScripti.Firlat(this);
        }

        // 4. Atış durumunu kapalı (Meşgul) yap.
        atisHazir = false;
    }

    // Yeni atış için bekleme süresi bittiğinde çağrılır.
    void AtisHazirYap()
    {
        atisHazir = true;
        print("Atış hazır yap'tan çıkıyor.");
    }
    public void TorpidoGeriGetir(GameObject torpido)
    {
        // Geri gelen objenin doğru objemiz olduğundan emin ol.
        if (torpido == null || torpido != torpidoObjesi) return;

        // 1. Geri Bağlama (Reparenting):
        // Torpidoyu tekrar Denizaltı objesinin çocuğu yap.
        torpidoObjesi.transform.parent = this.transform;

        // 2. Pozisyonu Sıfırlama:
        // Torpidoyu burnun başlangıç pozisyonuna geri getir.
        torpidoObjesi.transform.localPosition = Vector3.zero;
        torpidoObjesi.transform.localRotation = Quaternion.identity; // Rotasyonu sıfırla.

        // 3. Pasif Hale Getirme (Görünmez yapma):
        torpidoObjesi.SetActive(false);

        // 4. Yeni atış iznini ver.
        atisHazir = true;

        Debug.Log("Torpidonun ömrü bitti, geri getirildi ve atış hazır.");
    }
}