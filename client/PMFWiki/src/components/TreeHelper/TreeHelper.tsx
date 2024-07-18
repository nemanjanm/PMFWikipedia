import { years } from "../Years";
import { semestars } from "../Years";
import "./TreeHelper.css"

export function TreeHelper(props : [])
{   
    const tree : any = [];
    console.log(props);
    years.forEach((y: any, index: number) => {
        const children = semestars.map((s: any, index2: number) => {
            const subjects = props.filter((p: any) => 
                p.year == y.id && p.semester == s.id 
            ).map((p: any, index3: number) => {
                return {
                    key: index +"-"+ index2 +"-"+ index3,
                    label: <a href="/profilna-strana" className="" style={{padding: "20px"}}>{p.name}</a>,
                    id: p.id,
                    url: "/profilna-strana"
                }
            })
            return {
                key: index +"-"+ index2,
                label: s.semestar,
                id: s.id,
                children: subjects
            }
        })
        tree.push({key : index, label: y.year, id: y.id, children: children
        });
    });
    
    return tree;
}