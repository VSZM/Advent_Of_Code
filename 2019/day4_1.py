

FROM = 171309
TO = 643603

def isPasswordCandidate(num):
    string = str(num)
    
    prev = string[0]
    has_twin = False

    for i in string[1:]:
        if i < prev:
            return False
        if i == prev:
            has_twin = True    
        prev = i

    return has_twin

password_candidates = 0
for num in range(FROM,TO):
    if isPasswordCandidate(num):
        password_candidates += 1

print(password_candidates)