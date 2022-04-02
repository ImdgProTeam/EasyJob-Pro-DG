﻿using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Dg
    {
        /// <summary>
        /// Method to assign segregaion group from Ch 3.3
        /// </summary>
        public void AssignSegregationGroup()
        {
            int[] acids = //1
            {
                1052, 1182, 1183, 1238, 1242, 1250, 1295, 1298, 1305, 1572, 1595, 1715, 1716, 1717,
                1718, 1722, 1723, 1724, 1725, 1726, 1727, 1728, 1729, 1730, 1731, 1732, 1733, 1736,
                1737, 1738, 1739, 1740, 1742, 1743, 1744, 1745, 1746, 1747, 1750, 1751, 1752, 1753,
                1754, 1755, 1756, 1757, 1758, 1762, 1763, 1764, 1765, 1766, 1767, 1768, 1769, 1770,
                1771, 1773, 1775, 1776, 1777, 1778, 1779, 1780, 1781, 1782, 1784, 1786, 1787, 1788,
                1789, 1790, 1792, 1793, 1794, 1796, 1798, 1799, 1800, 1801, 1802, 1803, 1804, 1805,
                1806, 1807, 1808, 1809, 1810, 1811, 1815, 1816, 1817, 1818, 1826, 1827, 1828, 1829,
                1830, 1831, 1832, 1833, 1834, 1836, 1837, 1838, 1839, 1840, 1848, 1873, 1898, 1902,
                1905, 1906, 1938, 1939, 1940, 2031, 2032, 2214, 2215, 2218, 2225, 2226, 2240, 2262,
                2267, 2305, 2308, 2331, 2353, 2395, 2407, 2434, 2435, 2437, 2438, 2439, 2440, 2442,
                2443, 2444, 2475, 2495, 2496, 2502, 2503, 2506, 2507, 2508, 2509, 2511, 2513, 2531,
                2564, 2571, 2576, 2577, 2578, 2580, 2581, 2582, 2583, 2584, 2585, 2586, 2604, 2626,
                2642, 2670, 2691, 2692, 2698, 2699, 2739, 2740, 2742, 2743, 2744, 2745, 2746, 2748,
                2751, 2789, 2790, 2794, 2796, 2798, 2799, 2802, 2817, 2819, 2820, 2823, 2826, 2829,
                2834, 2851, 2865, 2869, 2879, 2967, 2985, 2986, 2987, 2988, 3246, 3250, 3260, 3261,
                3264, 3265, 3277, 3361, 3362, 3412, 3419, 3420, 3421, 3425, 3453, 3456, 3463, 3472,
                3498
            };

            int[] ammoniumCompounds = //2
            {
                4, 222, 402, 1310, 1439, 1442, 1444, 1512, 1546, 1630, 1727, 1835, 1843, 1942, 2067,
                2071, 2073, 2426, 2505, 2506, 2683, 2687, 2817, 2818, 2854, 2859, 2861, 2863, 3375,
                3423, 3424
            };

            int[] bromates = //3
            {
                1450, 1473, 1484, 1494, 2469, 2719, 3213
            };

            int[] chlorates = //4
            {
                1445, 1452, 1458, 1459, 1461, 1485, 1495, 1506, 1513, 2427, 2428, 2429, 2573, 2721, 2723, 3405, 3407
            };

            int[] chlorites = //5
            {
                1453, 1462, 1496, 1908
            };

            int[] cyanides = //6
            {
                1541, 1565, 1575, 1587, 1588, 1620, 1626, 1636, 1642, 1653, 1679, 1680, 1684, 1689, 1694,
                1713, 1889, 1935, 2205, 2316, 2317, 3413, 3414, 3449
            };

            int[] heavyMetals = //7
            {
                129, 130, 135, 1347, 1389, 1392, 1435, 1436, 1469, 1470, 1493, 1512, 1513,
                1514, 1515, 1516, 1587, 1616, 1617, 1618, 1620, 1623, 1624, 1625, 1626, 1627, 1629, 1630,
                1631, 1634, 1636, 1637, 1638, 1639, 1640, 1641, 1642, 1643, 1644, 1645, 1646, 1649, 1653,
                1674, 1683, 1684, 1712, 1713, 1714, 1794, 1838, 1840, 1872, 1894, 1895, 1931, 1931, 2024,
                2025, 2026, 2291, 2331, 2441, 2469, 2546, 2714, 2777, 2778, 2809, 2855, 2869, 2878, 2881,
                2989, 3011, 3012, 3089, 3174, 3181, 3189, 3401, 3402, 3408, 3483
            };

            int[] hypochlorites = //8
            {
                1471, 1748, 1791, 2208, 2741, 2880, 3212, 3255, 3485, 3486, 3487
            };

            int[] lead = //9
            {
                129, 130, 130, 1469, 1470, 1616, 1617, 1618, 1620, 1649, 1794, 1872, 2291, 2989, 3408, 3483
            };

            int[] hydrocarbons = //10
            {
                1099, 1100, 1107, 1126, 1127, 1134, 1150, 1152, 1184, 1278, 1279, 1303, 1591, 1593, 1605,
                1647, 1669, 1701, 1702, 1710, 1723, 1737, 1738, 1846, 1887, 1888, 1891, 1897, 1991, 2234,
                2238, 2279, 2321, 2322, 2339, 2341, 2342, 2343, 2344, 2356, 2362, 2387, 2388, 2390, 2391,
                2392, 2456, 2504, 2515, 2554, 2644, 2646, 2664, 2688, 2831, 2872
            };

            int[] mercury = //11
            {
                135, 1389, 1392, 1623, 1624, 1625, 1626, 1627, 1629, 1630, 1631, 1634, 1636, 1637, 1638,
                1639, 1640, 1641, 1642, 1643, 1644, 1645, 1646, 1894, 1895, 2024, 2025, 2026, 2777, 2778,
                2809, 3011, 3012, 3401, 3402
            };

            int[] nitrites = //12
            {
                1487, 1488, 1500, 1512, 2627, 2726, 3219
            };

            int[] perchlorates = //13
            {
                1442, 1447, 1455, 1470, 1475, 1481, 1489, 1502, 1508, 3211, 3406, 3408
            };

            int[] permanganates = //14
            {
                1448, 1456, 1482, 1490, 1503, 1515, 3214
            };

            int[] powderedMetals = //15
            {
                1309, 1326, 1352, 1358, 1383, 1396, 1398, 1418, 1435, 1436, 1854, 2008, 2009, 2545, 2546, 2878,
                2881, 2950, 3078, 3089, 3170, 3189
            };

            int[] peroxides = //16
            {
                1449, 1457, 1472, 1476, 1483, 1491, 1504, 1509, 1516, 2014, 2015, 2466, 2547, 3149, 3377, 3378
            };

            int[] azides = //17
            {
                129, 224, 1571, 1687
            };

            int[] alkalis = //18
            {
                1005, 1160, 1163, 1235, 1244, 1382, 1385, 1604, 1719, 1813, 1814, 1819, 1823, 1824, 1825,
                1835, 1847, 1849, 1907, 1922, 2029, 2030, 2033, 2073, 2079, 2259, 2270, 2318, 2320, 2379,
                2382, 2386, 2399, 2401, 2491, 2579, 2671, 2672, 2677, 2678, 2679, 2680, 2681, 2682, 2683,
                2733, 2734, 2735, 2795, 2797, 2818, 2949, 3028, 3073, 3253, 3259, 3262, 3263, 3266, 3267,
                3293, 3318, 3320, 3423, 3484
            };

            int[] strongAcids = //19
            {
                1052, 1777, 1786, 1787, 1788, 1789, 1790, 1796, 1798, 1802, 1826,
                1830, 1831, 1832, 1873, 1906, 2031, 2032, 2240, 2308, 2796
            };

            if (acids.Contains(Unno))
                segregationGroup.Add(1);
            if (ammoniumCompounds.Contains(Unno))
                segregationGroup.Add(2);
            if (bromates.Contains(Unno))
                segregationGroup.Add(3);
            if (chlorates.Contains(Unno))
                segregationGroup.Add(4);
            if (chlorites.Contains(Unno))
                segregationGroup.Add(5);
            if (cyanides.Contains(Unno))
                segregationGroup.Add(6);
            if (heavyMetals.Contains(Unno))
                segregationGroup.Add(7);
            if (hypochlorites.Contains(Unno))
                segregationGroup.Add(8);
            if (lead.Contains(Unno))
                segregationGroup.Add(9);
            if (hydrocarbons.Contains(Unno))
                segregationGroup.Add(10);
            if (mercury.Contains(Unno))
                segregationGroup.Add(11);
            if (nitrites.Contains(Unno))
                segregationGroup.Add(12);
            if (perchlorates.Contains(Unno))
                segregationGroup.Add(13);
            if (permanganates.Contains(Unno))
                segregationGroup.Add(14);
            if (powderedMetals.Contains(Unno))
                segregationGroup.Add(15);
            if (peroxides.Contains(Unno))
                segregationGroup.Add(16);
            if (azides.Contains(Unno))
                segregationGroup.Add(17);
            if (alkalis.Contains(Unno))
                segregationGroup.Add(18);
            if (strongAcids.Contains(Unno))
                segregationGroup.Add(19);
        }
    }
}