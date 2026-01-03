using UnityEngine;

public class TorpidoHareketi : MonoBehaviour
{
    // Deðiþkenler
    public float hiz = 20f;
    public float omurSuresi = 5f;
    public float hasarMiktari = 20f; // Torpidonun vereceði hasar

    // Denizaltý Kontrol script'ine referans
    private DenizAltiKontrol anaKontrol;

    // Denizaltý, torpidoyu fýrlatýrken bu metodu çaðýracak.
    // 'kontrolScripti' deðiþkeni, DenizAltiKontrol objesinin referansýdýr.
    public void Firlat(DenizAltiKontrol kontrolScripti)
    {
        // Kontrol referansýný kaydet
        anaKontrol = kontrolScripti;

        // Belirlenen süre (omurSuresi) sonunda "OmurBitti" metodunu çaðýr.
        Invoke("OmurBitti", omurSuresi);
    }

    void FixedUpdate()
    {
        // Torpidoyu kendi Y ekseninde (transform.up) sürekli hareket ettir.
        transform.position += transform.up * hiz * Time.fixedDeltaTime;
    }

    // Torpidonun ömrü bittiðinde çaðrýlan metot.
    void OmurBitti()
    {
        if (anaKontrol != null)
        {
            // Kontrolöre, torpidonun geri getirilmesi gerektiðini söyle.
            // "gameObject" ile bu torpidonun kendisini gönderiyoruz.
            anaKontrol.TorpidoGeriGetir(gameObject);
        }
    }
    // Unity'nin çarpýþma algýlayýcýsý metodu.
    // Torpido bir objeye çarptýðýnda otomatik olarak çaðrýlýr.
    void OnCollisionEnter(Collision collision)
    {
        // 1. Çarptýðýmýz objeyi al.
        GameObject carpanObje = collision.gameObject;

        // 2. Çarptýðýmýz objede CanYonetimi script'i var mý kontrol et.
        CanYonetimi canYonetimi = carpanObje.GetComponent<CanYonetimi>();

        // 3. Eðer CanYonetimi varsa hasar ver.
        if (canYonetimi != null)
        {
            // Hasar miktarýný negatif olarak göndererek caný azalt.
            canYonetimi.CaniDegistir(-hasarMiktari);

            // Debug.Log ile kontrol edelim.
            Debug.Log("Torpidomuz " + carpanObje.name + " objesine vurdu ve hasar verdi.");
        }

        // 4. Torpidonun iþi bitti, kendini yok et.
        // Ancak bu bir 'pooling' sistemi olduðu için yok etmek yerine geri getireceðiz.

        // **DÜZELTME:** Torpidonun ömrü bitmeden çarpýþma olursa, OmurBitti mantýðýný kullanmalýyýz.
        // Torpidoyu hemen yerine geri getiririz:
        if (anaKontrol != null)
        {
            // Bekleyen 'OmurBitti' çaðrýsýný iptal et (eðer varsa).
            CancelInvoke("OmurBitti");

            // Kontrolöre torpidonun geri getirilmesi gerektiðini söyle.
            anaKontrol.TorpidoGeriGetir(gameObject);
        }
        else
        {
            // Eðer nedense kontrol referansý kaybolduysa ve pool sistemi çalýþmýyorsa
            // (çok nadir bir durum), o zaman kendini yok etsin.
            Destroy(gameObject);
        }
    }
}