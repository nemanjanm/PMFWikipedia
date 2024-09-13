export const programmes= [
    {name: "Informatika", id: 1},
    {name: "Matematika", id: 2},
    {name: "Biologija", id: 3},
    {name: "Ekologija", id: 4},
    {name: "Fizika", id: 5},
    {name: "Hemija", id: 6},
    {name: "Psigologija", id: 7}
];

export function getName(id : any) : any{
    switch(id) {
        case 1:
            return "Informatika";
        case 2:
            return "Matematika";
        case 3:
            return "Biologija";
        case 4:
            return "Ekologija";
        case 5:
            return "Fizika";
        case 6:
            return "Hemija";
        case 7:
            return "Psihologija";
    }
}
