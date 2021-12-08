import numpy as np
import copy
import math

def lineToPlanet(line):
    clean_str = lambda string: string.replace('<', '').replace('>', '').replace('x=', '').replace('y=', '').replace('z=', '')
    line_split = clean_str(line).split(', ')
    return { 'posx': int(line_split[0]), 'posy': int(line_split[1]), 'posz': int(line_split[2]), 'velx': 0, 'vely': 0, 'velz': 0}

def getTotalEnergy(planets, steps):

    while steps > 0:
        # apply gravity
        for planet in planets:
            velx = 0
            vely = 0
            velz = 0
            for other in planets:
                velx += np.sign(other['posx'] - planet['posx'])
                vely += np.sign(other['posy'] - planet['posy'])
                velz += np.sign(other['posz'] - planet['posz'])
            planet['velx'] += velx
            planet['vely'] += vely
            planet['velz'] += velz


        
        # apply velocity
        for planet in planets:
            planet['posx'] += planet['velx']
            planet['posy'] += planet['vely']
            planet['posz'] += planet['velz']
        steps -= 1

    summa = 0
    # calculate total energies
    for planet in planets:
        planet['pot'] = np.abs(planet['posx']) + np.abs(planet['posy']) + np.abs(planet['posz'])
        planet['kin'] = np.abs(planet['velx']) + np.abs(planet['vely']) + np.abs(planet['velz'])
        planet['total'] = planet['pot'] * planet['kin']
        summa += planet['total']

    return summa

def lcm(a,b):
    return a*b // math.gcd(a,b)

def getCycleLength(planets):
    xcycle = None
    ycycle = None
    zcycle = None
    histx = set()
    histy = set()
    histz = set()


    while xcycle == None or ycycle == None or zcycle == None:
        # update history
        statex = tuple([(planet['posx'], planet['velx']) for planet in planets])
        statey = tuple([(planet['posy'], planet['vely']) for planet in planets])
        statez = tuple([(planet['posz'], planet['velz']) for planet in planets])
        if statex in histx:
            xcycle = len(histx)
        else:
            histx.add(statex)

        if statey in histy:
            ycycle = len(histy)
        else:
            histy.add(statey)
        
        if statez in histz:
            zcycle = len(histz)
        else:
            histz.add(statez)

        # apply gravity
        for planet in planets:
            velx = 0
            vely = 0
            velz = 0
            for other in planets:
                velx += np.sign(other['posx'] - planet['posx'])
                vely += np.sign(other['posy'] - planet['posy'])
                velz += np.sign(other['posz'] - planet['posz'])
            planet['velx'] += velx
            planet['vely'] += vely
            planet['velz'] += velz

        # apply velocity
        for planet in planets:
            planet['posx'] += planet['velx']
            planet['posy'] += planet['vely']
            planet['posz'] += planet['velz']

    lcm_nums = [xcycle, ycycle, zcycle]
    answer = lcm_nums[0]
    for num in lcm_nums[1:]:
        answer = lcm(answer, num)

    return answer


if __name__ == "__main__":
    with open('day12.txt', 'r') as f:
        lines = f.readlines()
        planets = [lineToPlanet(line)  for line in lines]

    print(getTotalEnergy(copy.deepcopy(planets), 1000))
    print(getCycleLength(copy.deepcopy(planets)))