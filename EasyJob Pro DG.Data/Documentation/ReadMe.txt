PLEASE READ SOME IMPORTANT NOTES BELOW FOR BETTER UNDERSTANDING OF "PRO DG" PERFORMANCE CAPABILITIES

VERSION: IMDG CODE 2022 Edition, Amendments 41-22.

The "Pro DG" software works with .edi or excel files and creates a container list. Then checking of stowage and segregation according to IMDG code and taking into account ship design and other cargo will be carried out. Finally, the list of possible conflicts will be displayed. That is the responsibility of the user to make a decision referring to advice given by "Pro DG" and based on the all other available information, such as cargo manifests, charterer's/owners instructions, better knowledge of vessel design and, of course, own experience. The only result of the software cannot be deemed as the final one.

* No segregation is checked for dg cargoes inside the same cargo transport unit.
* Segregation of cargoes is checked in accordance with IMDG code regulations for closed cargo holds with watertight or watertight hatch covers with effective gutter bars. Stowage and segregation check for hatchless container ships or where hatch covers are without effective     gutter bars is not implemented.
* For proper segregation and stowage, checking ship settings shall be set up carefully.
* For the purpose of segregation only bulkheads between the cargo holds are taken into account and are assumed to be in compliance with the provisions of SOLAS and the Code (i.e. e.g. resistant to fire and liquids). Assumed that no cargo hold has such a bulkhead segregating cargo inside the same hold either athwartships or longitudinally. 
* For "separated longitudinally by an intervening complete compartment or hold from" (segregation case 4) "minimum horizontal distance of 24 m" assumed as four container slots, "6 m from intervening bulkhead" as one container space. No spacing between containers or bays longitudinally taken into account. That is possible that on particular ship a bigger gap may exist between two hatch covers. Such gap is not taken into account by the software.
* Segregation of explosives (class 1) is made with compatibility groups as directed by table 7.2.7.1.4 and rule 7.2.7.1.5. No remarks of table 7.2.7.1.4 are taken into account, i.e. wherever there is a remark segregation of containers will give a conflict unless "separated from" each other. If only substances with compatibility groups S and G are there on board, then segregation requirements are assumed to be fully met.
* Additionally, regulations do not specify segregation of explosives in open cargo transport units. In the case, conflict will be shown if stowed in the same compartment or on deck, unless permitted to be stowed together according to table 7.2.7.1.4. 
* For stowage and segregation, checking ONLY information contained in BAPLIE .edi file is used, i.e. DG commodity of containers, its UNNO, class, packing group and flash point, provided that the data available in .edi file and correct. Then the data will be updated with information contained in IMDG code Ch. 3. Any information, such as LQ, MP, specified segregation group etc. missing in .edi file, not correct against the manifest or provided in specific format agreed by particular charterer will not be considered by the software.
* All users are most welcome to advise any troubles, mistakes, uncertainties and suggestions to creators of this software in order to improve its usability. On voluntary basis, please send your notes along with .edi files, especially those causing troubles, to let us test more different versions and make the software universal.
* Unless clearly specified in .edi file, AEROSOLS (unno 1950) and RECEPTACLES (unno 2037) will get class 2.1 without any subrisk by default. Refer to manifest and to special provision 63 of ch.3 (for AEROSOLS). Same warning will be issued for unno 1950 only.
* Most common container types are recognized automatically weather it is closed freight container or not. If a container type is not recognized by the software, a warning will be given and by default, the container will be considered as 'closed freight container'.
* For the purpose of segregation of classes 2.1 and 3 with reefers, any container height considered as 2.5 m. If flash point not specified, a conflict will be given if such classes located in direct proximity of reefers. Safe approved type of reefers to be considered by user only, software gives alarm irrespective of type.

Segregation takes into account:
- DG class
- DG subclass
- Special segregation provision as stated in column 16b of Ch. 3 according to unno
- Segregation group according to Ch. 3.1.4
- Type of container (open/closed)
- Proximity to reefers
- Limited quantities

* All temperatures are given in degrees Celsius. When reading from .edi file if Fahrenheit it will be converted into Celsius. From Excel it will be read directly as Celsius.

* Working with Excel:
Caution!!! When importing Excel table, regional language settings may play a critical role. It is recommended to store classes and subclasses as text and ensure correct separator is used.
By default, all containers imported from Excel table are closed type. User can add the word 'open' in remark column to make unit be imported as open (not closed freight) container.


All your findings and/or suggestions are most welcome on feedback@imdg.pro

JUST REMEMBER! THE FINAL DECISION IS ALWAYS BEHIND THE USER!
