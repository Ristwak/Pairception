using UnityEngine;
using TMPro;

/// <summary>
/// Populates the About panel with det,vkbZled game info for P,vkbZrception.
/// </summary>
public class AboutGameManager : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Assign the TMP_Text component for the About section.")]
    public TMP_Text aboutText;

    void Start()
    {
        if (aboutText == null)
        {
            Debug.LogError("AboutGameManager: 'aboutText' reference is missing.");
            return;
        }

        aboutText.text =
            "<b>is;jlsI'ku ds ckjs esa</b>\n" +
            "is;jlsI'ku ,d rst+ xfr okyk feyku xse gS ftls ,vkbZ vkSj e'khu yfuZax voèkkj.kkvksa dh vkidh le> dks pqukSrh nsus vkSj c<+kus ds fy, fMt+kbu fd;k x;k gSA le; lekIr gksus ls igys eq[; 'kCnksa dks mudh lgh ifjHkk\"kkvksa ls feyk,a!\n\n" +

            "<b>;g [ksy fdl ckjs esa gS\\</b>\n" +
            "çR;sd jkmaM esa 4Û4 d‚ye feyku fxzM çLrqr fd;k tkrk gSA vkidk dk;Z çR;sd ,vkbZ@,vkbZ,e ,y 'kCn dks ,vkbZ,e ,yds lgh Li\"Vhdj.k ds lkFk tksM+uk gSA\n" +
            "Dykfld ,Yxksfjne ls ysdj e‚My O;ogkj rd] ijlsI'ku dks Lej.k vkSj rdZ nksuksa dks rst djus ds fy, cuk;k x;k gSA\n\n" +

            "<b>is;jlsI'ku D;ksa\\</b>\n" +
            ";g Le`fr ls dgha vfèkd gS & ;g vuqHkwfr gSA\n" +
            "Rofjr] lgt feyku ds ekè;e ls ,vkbZ vkSj ,vkbZ,e ,y esa ewyHkwr fopkjksa dks igpkuus vkSj le>us dh viuh {kerk dks çf'kf{kr djsaA\n\n" +

            "<b>dksj xseIys</b>\n" +
            "• 'vksojfQfVax'] 'ihlh,'] 'th,,u'] vkSj vU; tSls 'kCnksa dk feyku djsa\n" +
            "• –'; js[kkvksa ds lkFk rRdky çfrfØ;k çkIr djsa ¼lgh ¾ gjk] xyr ¾ yky½\n" +
            "• [ksy&[ksy esa lh[ksa & ,d le; esa ,d voèkkj.kk\n\n" +

            "<b>vki D;k lh[ksaxs</b>\n" +
            "• e'khu yfuZax vkSj ,vkbZ esa çeq[k voèkkj.kk,¡\n" +
            "• rduhdksa] e‚Myksa vkSj ifj.kkeksa ds chp lacaèk\n" +
            "• tfVy 'kCnkoyh dh rhoz igpku\n\n" +

            "<b>Nk=ksa ds fy, fMt+kbu fd;k x;k</b>\n" +
            "is;jlsI'ku ihth f'k{kkfFkZ;ksa ds fy, vkn'kZ gS] fo'ks\"k :i ls tks fuEufyf[kr {ks=ksa esa vè;;u djuk pkgrs gSa:\n" +
            "• e'khu yfuZax dh cqfu;kn\n" +
            "• ,Yxksfjne O;ogkj vkSj 'kCnkoyh\n" +
            "• lfØ; [ksy ds ekè;e ls voèkkj.kk lq–<+hdj.k\n\n" +

            "<b>[kqfQ;k dksM dks rksM+sa</b>\n" +
            "viuh ,vkbZ@,vkbZ,e ,y IQ dks c<+k,¡ & ,d ckj esa ,d eSpA\n" +
            "'kCnksa ij egkjr gkfly djsaA voèkkj.kkvksa dks tksM+saA\n\n" +
            "<b>is;jlsI'ku esa vkidk Lokxr gSA</b>";
    }
}
