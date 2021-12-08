

FROM = 171309
TO = 643603

def isPasswordCandidate(num):
    string = str(num)
    
    prev = string[0]
    twin_sequence = [string[0]]
    has_bi_twin = False

    for i in string[1:]:
        if i < prev:
            return False
        if i == prev:
            twin_sequence.append(i)
        else:
            if len(twin_sequence) == 2:
                has_bi_twin = True
            twin_sequence = [i]
        prev = i

    if len(twin_sequence) == 2:
        has_bi_twin = True

    return has_bi_twin

password_candidates = 0
for num in range(FROM,TO):
    if isPasswordCandidate(num):
        password_candidates += 1

print(password_candidates)