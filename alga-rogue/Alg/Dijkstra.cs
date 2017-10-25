using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Alg
{
    static class Dijkstra
    {
        public static List<Chamber> ShortestPath(this Dungeon dungeon, Chamber start, Chamber finish)
        {
            dungeon.NotVisited();

            var verticies = dungeon.ToDictionary(
                key => key,
                el => el.Where(x => x.Passable).ToDictionary(x => x.To, x => (int)x.Weight)
            );

            var previous = new Dictionary<Chamber, Chamber>();
            var distances = dungeon.ToDictionary(x => x, elementSelector: el => { if (el == start) return 0; return int.MaxValue; });
            var nodes = verticies.Select(x => x.Key).ToList();

            List<Chamber> path = null;

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<Chamber>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                    break;

                foreach (var neighbor in verticies[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;

                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            foreach (var node in path)
            {
                node.WasVisitedForSearch = true;
            }

            return path;
        }
    }
}
