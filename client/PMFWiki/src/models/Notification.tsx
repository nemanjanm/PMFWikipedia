export const notifications= [
    {name: "Kolkovijum", id: 1},
    {name: "Ispit", id: 2},
    {name: "Post", id: 3},
    {name: "Komentar na tvoj post", id: 4},
    {name: "Komentar gde si ti komentarisao", id: 5},
    {name: "Izmenjen je tvoj post", id: 6},
    {name: "Resenje Kolokvijuma", id: 7},
    {name: "Resenje Ispita", id: 8}
];

export function getNotName(id : any) : any{
    switch(id) {
        case 1:
            return "Kolkovijum";
        case 2:
            return "Ispit";
        case 3:
            return "Post";
        case 4:
            return "Komentar na tvoj post";
        case 5:
            return "Komentar gde si ti komentarisao";
        case 6:
            return "Izmenjen je tvoj post";
        case 7:
            return "Resenje Kolokvijuma";
        case 8:
            return "Resenje Ispita";
    }
}
