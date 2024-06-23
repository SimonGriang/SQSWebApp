# 

**ARC42 Dokumentation WebApp**

Diese Markdown Datei dient der ARC42 Dokumentation für das SQS-Projekt.


# Einführung und Ziele {#section-introduction-and-goals}

Dieses Dokument beschreibt die Architektur der .NET Core MVC-Anwendung, die als Übersetzer fungiert und die DEEPL-API nutzt. Zusätzlich speichert die Anwendung alle Übersetzungen in einer Datenbank. Dieses Projekt wurde im Rahmen einer Vorlesung an der Hochschule erstellt und wird von meinem Dozenten bewertet.

## Aufgabenstellung {#_aufgabenstellung}

Aufgabe it es eine Anwendung zu erstellen. Die Qualität der Software soll professionell abgesichert werden. Die wesentlichsten vorgegebenen Bedingungen für die Umsetzung sind:
- das Verwenden einer externen API
- das Einbinden einer Datenbank
- das Integrieren einer Benutzeroberfläche
 
## Qualitätsziele {#_qualit_tsziele}

Die folgend aufgeführten Qualitätsziele nach ISO 25010 sind: Functional Suitability, Operability, Maintainability

## Stakeholder {#_stakeholder}

## Stakeholder {#_stakeholder}

| Rolle           | Kontakt                                   | Erwartungshaltung                            |
|:=================:|:===========================================:|:==============================================:|
| *Student *      | *simon.goettsberge@stud.th-rosenheim.de*  | *Eigener Lernerfolg*                         | 
| *Dozent*        | *mario-leander.reimer@th-rosenheim.de*    | *Umsetzung einer fundierten und professionellen Software-Qualitätssicherung* |



# Randbedingungen {#section-architecture-constraints}
Die Randbedingungen dieses Projektes werden ausschließlich durch die mündlich kommunizierten Anforderungen bestimmt. Dabei gilt es eine Software zu entwerfen die gezielt entworfen wird. Dabei muss die Software eine externe API, als auch eine Datenbank verwenden. Weitere Details sind alleine dem Studenten überlassen, solange die Software, nach Einschätzung des Dozenten, angemessenen Aufwand qualitätssichernde Maßnahmen genutzt werden. 

# Kontextabgrenzung {#section-system-scope-and-context}
![Image Description](images/KomponentenDiagramm.png)

Der User kann in der Applikation Übersetzungen durchführen. Dazu stehen ihm alle Sprachen zur Verfügung die auch DeepL anbietet. Alle durchgeführten Übersetzungen werden in der Datenbank gespeichert. Die gespeicherten Übersetzungen können eingesehen werden. Dabei zeigt sich Ausgangs- und Übersetzungssprache also auch der Original- und Übersetzungstext. Zusätzlich wird auch angezeigt wann die Übersetzung durchgeführt wurde. 

Um die Übersetzungen durchzuführen wird auf die DeepL Api zurückgegriffen.

Die durchgeführten Übersetzungen werden auf einer relationalen Datenbank gespeichert.

## Fachlicher Kontext {#_fachlicher_kontext}

|    Element   |                                                          Description                                                         |
|:------------:|:----------------------------------------------------------------------------------------------------------------------------:|
|     User     | Der User führt Eingaben bezüglich der gewünschten  Sprache und des zu übersetzenden Textes durch.                            |
|    WebApp    | Die .Net Core MVC Web Anwedung nimmt die ausgewählten Parameter entgegen und führt darauf die Übersetzung des Textes durch.  |
| Build System | .Net                                                                                                                         |
|     DeepL    | DeepL ist der externe Übersetzungsdienst der für die tatsächliche Durchführung der Übersetzung verwendet wird.               |
|   Datenbank  | Die verwendete Datenbank speichert die durchgeführten Übersetzungen ab.                                                      |

## Technischer Kontext {#_technischer_kontext}

**\<Diagramm oder Tabelle>**

**\<optional: Erläuterung der externen technischen Schnittstellen>**

**\<Mapping fachliche auf technische Schnittstellen>**

# Lösungsstrategie {#section-solution-strategy}

.NET Core MVC ist ein leistungsstarkes, quelloffenes Framework, das eine robuste und skalierbare Plattform für die Webentwicklung bietet. Seine modulare Architektur ermöglicht einfache Wartung und Erweiterung. Ein großer Vorteil von .NET Core ist, dass alles in einer Hand liegt: Die gesamte Anwendung besteht aus einem Projekt und muss nicht aus mehreren Projekten zusammengeführt werden, was die Entwicklung beschleunigt, ohne die Qualität zu beeinträchtigen. Außerdem sorgen die plattformübergreifenden Fähigkeiten von .NET Core dafür, dass unsere Anwendung auf verschiedenen Betriebssystemen laufen kann, was entscheidend ist, um ein breiteres Publikum zu erreichen. Das MVC-Muster (Model-View-Controller) gewährleistet eine saubere Trennung der Anliegen, was die Code-Organisation erleichtert und die Wartbarkeit der Anwendung verbessert.

DeepL wird aufgrund seiner überlegenen Übersetzungsqualität ausgewählt, die für die Bereitstellung genauer und natürlicher Übersetzungen entscheidend ist. Die API-Integration von DeepL ist unkompliziert und gut dokumentiert, was eine nahtlose Einbindung in unser System ermöglicht. Durch die Nutzung von DeepL können wir uns auf die Entwicklung der Anwendungslogik und Benutzeroberfläche konzentrieren, anstatt eine eigene Übersetzungsengine entwickeln zu müssen.

Die Verwendung von PostgreSQL bietet zahlreiche Vorteile, darunter hohe Stabilität, Transaktionsunterstützung und eine breite Palette von erweiterten Funktionen für komplexe Datenbankanforderungen. Docker-Container mit PostgreSQL ermöglichen eine flexible und konsistente Umgebungsbereitstellung für Entwicklung, Tests und Bereitstellung, unabhängig von der lokalen Entwicklungsumgebung. Dies fördert eine konsistente Datenbankverwaltung.


# Bausteinsicht {#section-building-block-view}

## Whitebox Gesamtsystem {#_whitebox_gesamtsystem}

![Gesamtsystem](images/KomponentenDiagramm.png)

Enthaltene Bausteine

|    Element   |                                                          Description                                                         |
|:------------:|:----------------------------------------------------------------------------------------------------------------------------:|
|    WebApp    | Die .Net Core MVC Web Anwedung nimmt die ausgewählten Parameter entgegen und führt darauf die Übersetzung des Textes durch.  |
|     DeepL    | DeepL ist der externe Übersetzungsdienst der für die tatsächliche Durchführung der Übersetzung verwendet wird.               |
|   Datenbank  | Die verwendete Datenbank speichert die durchgeführten Übersetzungen ab.                                                      |


## WebApp
Die Komponente WebApp enhät sämtliche Funktionalitäten unst stellt so das eigentliche Programm dar. Aufgabe dieser Komponente ist das Interagieren mit dem Benutzer über eine Benutzeroberfläche. Zudem ist auch die gesamte Geschäftslogik in dieser Komponente implementiert. Das Aufbereiten, Laden, Speichern und Löschen von Daten wird in dieser Komponente behandelt. Zudem werden auch die Nutzereingaben für die gewünschte Übersetzung an die DeepL API weitergereicht. Die Übersetzung wird entgegengenommen und entsprechend verarbeitet. 

## Ebene 2 {#_ebene_2}

![Teilkommponenten](Bausteinansicht_Ebene2.jpg)

Die WebApp Komponente besteht

| Komponente            | Aufgabe                                                                                       |
|-----------------------|-----------------------------------------------------------------------------------------------|
| Views                 | Stellt die Benutzerschnittstelle dar. Hier wird die Interaktion mit dem Benutzer ermöglicht. Diese Komponente ist für die Darstellung der Benutzeroberfläche verantwortlich. Sie zeigt Daten an und nimmt Benutzereingaben entgegen.   |
| Controller            | Der Controller agiert als Vermittler zwischen der Benutzeroberfläche (View) und den Geschäftslogik-Modellen. Er verarbeitet Benutzereingaben und ruft entsprechende Aktionen auf den Modellen oder Repositories auf.      |
| Repositories          | Diese Komponente ist für die Kommunikation mit der Datenbank zuständig. Sie führt CRUD-Operationen (Create, Read, Update, Delete) auf der Postgres-Datenbank durch.        |
| Models                | Modelle repräsentieren die Datenstruktur und enthalten die Daten der Anwendung. Sie sind das zentrale Datenobjekt, mit dem die Anwendung arbeitet.                         |
| TranslationService    | Dieser Dienst ermöglicht die Übersetzung von Texten durch die Anbindung an die externe DeepL API.                                   |
| ViewModel             | Das ViewModel stellt sicher, dass die Daten korrekt zwischen der View und den Modellen gebunden sind.|



## Ebene 3 {#_ebene_3}
![Teilkommponenten](Bausteinansicht_Ebene3.jpg)

| Komponente                  | Aufgabe                                                                                                   |
|-----------------------------|-----------------------------------------------------------------------------------------------------------|
| Views                       | Stellt verschiedene Benutzeroberflächen für Details, Index, Delete und History bereit.                    |
| HomeController              | Vermittelt zwischen der Benutzeroberfläche und den Geschäftslogik-Modellen. Verarbeitet Benutzereingaben. |
| TranslationService          | Bietet Übersetzungsdienste durch Anbindung an die DeepL API.                                              |
| TranslationWrapper          | Wrapper für den eigentlichen Übersetzer-Dienst (Translator), um API-Aufrufe zu kapseln.                   |
| Translator                  | Der eigentliche Dienst, der mit der DeepL API kommuniziert und Übersetzungen durchführt.                  |
| TranslationRepository       | Verantwortlich für CRUD-Operationen auf Translation-Entitäten in der Postgres-Datenbank.                  |
| LanguageRepository          | Verantwortlich für CRUD-Operationen auf Language-Entitäten in der Postgres-Datenbank.                     |
| Language                    | Repräsentiert eine Sprache mit Eigenschaften wie Name, Abkürzung und Flags für Ziel- und Ursprungssprache.|
| Translation                 | Repräsentiert eine Übersetzung mit Eigenschaften wie Originaltext, übersetzter Text und Zeitstempel.      |
| CreateTranslationViewModel  | Stellt die Datenbindung für die Erstellung von Übersetzungen bereit.                                      |
| CreateTranslationViewModelHandler | Verantwortlich für die Verarbeitung und Verwaltung von Daten im CreateTranslationViewModel.         |


### Whitebox Views

Details: Zeigt detaillierte Informationen zu einer bestimmten Entität an.
Index: Listet eine Übersicht von Entitäten auf.
Delete: Ermöglicht das Löschen einer Entität.
History: Zeigt die Historie oder Änderungen von Entitäten an.

### Whitebox Controller

HomeController steuert die Navigation und Interaktionen auf der Startseite.
Vermittelt zwischen Benutzerinteraktionen und der Geschäftslogik, indem es Anfragen an die entsprechenden Services und Repositories weiterleitet.

### Whitebox TranslationService

TranslationService: Der Hauptdienst für die Übersetzungslogik. Nutzt den TranslationWrapper zur Kommunikation mit externen APIs.
TranslationWrapper: Kapselt die Logik für den Zugriff auf den Translator-Dienst, um API-Aufrufe zu vereinfachen und zu standardisieren.
Translator: Der konkrete Dienst, der die Übersetzungen durchführt, indem er die DeepL API aufruft.

### Whitebox Repositories

TranslationRepository: Verwalten der Persistenz von Übersetzungsdaten (Translation-Entitäten) in der Datenbank.
LanguageRepository: Verwalten der Persistenz von Sprachdaten (Language-Entitäten) in der Datenbank.

### Whitebox Repositories
Language: Datenmodell, das eine Sprache repräsentiert, einschließlich Name, Abkürzung und Flags zur Identifizierung, ob es sich um eine Ziel- oder Ursprungssprache handelt.
Translation: Datenmodell, das eine Übersetzung repräsentiert, einschließlich Originaltext, übersetzter Text, Zeitstempel und Referenzen zu den zugehörigen Sprachen.

### Whitebox Repositories
CreateTranslationViewModel: Datenmodell für die Erstellung einer neuen Übersetzung. Enthält Listen von Ziel- und Ursprungssprachen sowie Informationen zur Übersetzung.
CreateTranslationViewModelHandler: Verantwortlich für die Verwaltung und Verarbeitung der Daten im CreateTranslationViewModel, einschließlich der Interaktion mit dem LanguageRepository zur Beschaffung von Sprachdaten.

# Laufzeitsicht {#section-runtime-view}

## Translation of User Text

![Sequenzdiagramm Translation of Text](Sequenzdiagram_Translate.jpg)

Der Prozess beginnt in der View-Schicht, die eine Anfrage zur Erstellung einer neuen Übersetzung an den Controller sendet. Der Controller nimmt die Anfrage entgegen und initiiert den Vorgang, indem er ein Datenmodell zur Erstellung der Übersetzung vorbereitet. Dieses Datenmodell wird an die Verwaltungslogik weitergeleitet, die notwendige Informationen aus den Repositories lädt. Nachdem die Verwaltungslogik die erforderlichen Informationen geholt hat, gibt sie das Modell an den Controller zurück. Der Controller übergibt das Modell an den Übersetzungsdienst, um die eigentliche Übersetzungsarbeit durchzuführen. Der Übersetzungsservice verarbeitet die eingehenden Daten und erstellt die Übersetzung.

Abschließend wird das Ergebnis der Übersetzung an die View-Schicht zurückgegeben, wo es dem Nutzer angezeigt wird.
## Laden von einer/mehreren Übersetzung/-en

![Laden von einer/mehreren Übersetzung/-en](Sequenzdiagram_GetTranslations.jpg)

Zuerst initiiert die View-Komponente die Aktion, indem eine Anfrage an den Controller sendet wird. Der Controller empfängt diese Anfrage und leitet sie an die Repositories weiter, dabei werden je nach Fall eine oder alle Übersetzungen aufgerufen. Sobald die Repositories die benötigte Information bereitgestellt haben, sendet sie eine Antwort an den Controller zurück. Der Controller übermittelt diese Antwort schließlich an die View, was den ursprünglichen Aufruf abschließt und die entsprechende Information an den User ausgibt. Dieses Sequenzdiagramm gilt sowohl für die Bereitstellung einer als auch mehrerer Überstetzungen, da sich der Workflow abgesehen von der Anzahl der geladenen Übersetzungen nicht unterscheidet.

## Löschen von Datenbankeinträgen

![Laden einer Übersetzung](Sequenzdiagram_Deletion.jpg)

Zunächst wird eine Anfrage von der View an den Controller gesendet, um eine Übersetzung zu laden. Der Controller übernimmt diese Anfrage und fordert dann die notwendigen Informationen vom Repository an. Nachdem der Controller die Informationen erhalten hat, initiiert er den Löschvorgang der Übersetzung im Repository. Nach erfolgreicher Löschung der Übersetzung wird eine Bestätigung an den Controller zurückgeführt.

# Verteilungssicht {#section-deployment-view}

ASP.NET Core MVC-Anwendungen müssen gehostet werden, um Nutzern über das Internet oder ein Intranet zur Verfügung zu stehen. Es gibt mehrere grundsätzliche Möglichkeiten, diese Anwendungen zu hosten. Eine verbreitete Methode ist die Verwendung von Kestrel, dem integrierten plattformübergreifenden Webserver von ASP.NET Core, der sowohl für Entwicklungs- als auch Produktionsumgebungen geeignet ist. Kestrel kann eigenständig oder hinter einem Reverse-Proxy-Server wie IIS, Nginx oder Apache betrieben werden, um zusätzliche Sicherheit und Skalierbarkeit zu bieten. IIS wird oft in Windows-Umgebungen verwendet, während Nginx und Apache in Linux-Umgebungen beliebt sind. Jede dieser Optionen bietet unterschiedliche Vorteile und Konfigurationsmöglichkeiten, abhängig von den spezifischen Anforderungen und der Infrastruktur der Anwendung.

# Querschnittliche Konzepte {#section-concepts}

In der ASP.NET Core MVC-Anwendung wurden mehrere wichtige Konzepte implementiert, die für die gesamte Architektur und die Qualität der Anwendung relevant sind. Diese Konzepte betreffen verschiedene Teile des Systems und sorgen für Konsistenz, Wartbarkeit und Skalierbarkeit.

## MVC-Architektur
Unsere Anwendung folgt dem Model-View-Controller (MVC) Architekturpattern. Die Trennung von Verantwortlichkeiten zwischen Modellen, Views und Controllern erleichtert die Wartung und Weiterentwicklung der Anwendung.

Controller: Die Controller sind für die Verarbeitung der Benutzereingaben verantwortlich, steuern den Anwendungsfluss und interagieren mit den Services. Ein spezifisches Beispiel ist die Nutzung eines externen Übersetzungsdienstes durch den Controller, um sprachspezifische Inhalte dynamisch zu laden.

Modelle: Unsere Modelle enthalten keine Geschäftslogik und dienen ausschließlich dem Datenaustausch. Sie repräsentieren die Struktur unserer Daten und werden für die Interaktion mit der Datenbank und den Views verwendet.

Views: Die Views sind für die Darstellung der Daten verantwortlich. Um eine klare Trennung der Darstellung von der Logik zu gewährleisten, verwenden wir ViewModels und ViewModelHandler. ViewModels enthalten die Daten, die eine View benötigt, und ViewModelHandler sind für die Erstellung und Verwaltung dieser ViewModels zuständig.

## Datenzugriff und Repositories
Für den Zugriff auf die Datenbank nutzen wir das Repository Pattern. Repositories bieten eine Abstraktionsschicht über den Datenzugriff und ermöglichen es uns, die Datenbankoperationen zentral zu verwalten. Dies trägt zu einer sauberen Trennung der Geschäftslogik und der Datenzugriffsschicht bei und erleichtert die Wartung und das Testen der Anwendung.

## Sprachverwaltung und Seeding
Bei jedem Programmstart werden die verfügbaren Sprachen in der Datenbank mittels eines Seeding-Prozesses aktualisiert. Dies stellt sicher, dass die Anwendung stets die aktuellen Sprachdaten zur Verfügung hat und erleichtert die Verwaltung der Lokalisierung. Der Seeding-Prozess überprüft und aktualisiert die in der Datenbank gespeicherten Sprachinformationen, um sicherzustellen, dass alle unterstützten Sprachen korrekt und aktuell sind.

# Architekturentscheidungen {#section-design-decisions}

# ADR 2: Implementierung der Webanwendung mit ASP.Net Core MVC

| **Section**   | **Description**                                                                                                                                              |
|---------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Title**     | ADR 2: Implementierung der Webanwendung mit ASP.Net Core MVC                                                                                                |
| **Context**   | Die Entscheidung betrifft die Auswahl der Entwicklungsplattform für unsere Webanwendung. Wir stehen vor verschiedenen technischen und politischen Herausforderungen. |
|               | - **Technischer Kontext**:                                                                                                                                 |
|               |   - ASP.Net Core MVC bietet eine moderne und flexible Plattform für die Entwicklung plattformübergreifender Webanwendungen.                                 |
|               |   - Die Architektur von MVC (Model-View-Controller) ermöglicht eine klare Trennung von Datenmodellen, Benutzeroberfläche und Geschäftslogik.                  |
|               |   - Der durchgängige Technologiestack von UI bis zur Datenbankanbindung minimiert Kompatibilitätsprobleme und erleichtert die Integration.                     |
|               | - **Politische Aspekte**:                                                                                                                                  |
|               |   - Entwickler verfügen über Erfahrung mit der MVC-Architektur, insbesondere mit Laravel, was die Einarbeitung in ASP.Net Core MVC erleichtert.               |
|               |   - Die Verwendung der etablierten Programmiersprache C# bietet Vorteile in Bezug auf Produktivität und Codequalität.                                       |
|               |   - Microsoft bietet umfangreiche Dokumentation, Tooling in Visual Studio IDE und Erweiterungen für Visual Studio Code, was die Entwicklung unterstützt.   |
|               |   - C# und die Unterstützung durch Entity Framework Core und MSTest bieten eine robuste Plattform für die Entwicklung und Wartung der Anwendung.             |
| **Decision**  | Wir entscheiden uns, die Webanwendung mit ASP.Net Core MVC zu implementieren, um von den technischen Vorteilen der Plattform und der vorhandenen Entwicklererfahrung zu profitieren. |
| **Status**    | Akzeptiert                                                                                                                                                   |
| **Consequences** | **Positive Konsequenzen**: <ul><li>Strukturierte Architektur durch die Nutzung von MVC.</li><li>Integrierter Technologiestack von UI bis Datenbankanbindung.</li><li>Unterstützung durch umfangreiche Microsoft-Dokumentation und Tools.</li><li>Verwendung einer etablierten Programmiersprache (C#).</li></ul><br> **Negative Konsequenzen**: <ul><li>Möglicherweise steilere Lernkurve für Entwickler ohne direkte Erfahrung mit ASP.Net Core MVC.</li><li>Abhängigkeit von der Weiterentwicklung und Unterstützung durch Microsoft.</li><br> **Neutrale Konsequenzen**: <ul><li>Keine spezifischen neutralen Konsequenzen identifiziert.</li></ul> |


# Qualitätsanforderungen {#section-quality-scenarios}

::: formalpara-title
**Weiterführende Informationen**
:::

Siehe [Qualitätsanforderungen](https://docs.arc42.org/section-10/) in
der online-Dokumentation (auf Englisch!).

## Qualitätsbaum {#_qualit_tsbaum}

## Qualitätsszenarien {#_qualit_tsszenarien}

# Risiken und technische Schulden {#section-technical-risks}

# Glossar {#section-glossary}

+-----------------------+-----------------------------------------------+
| Begriff               | Definition                                    |
+=======================+===============================================+
| *\<Begriff-1>*        | *\<Definition-1>*                             |
+-----------------------+-----------------------------------------------+
| *\<Begriff-2*         | *\<Definition-2>*                             |
+-----------------------+-----------------------------------------------+
